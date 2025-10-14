using BOZMANOHERMANO.Hub;
using BOZMANOHERMANO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StartUp.Models;
using StartUp.Models.Data;

namespace BOZMANOHERMANO.Services.Notifications
{
    public interface INotificationService
    {
        Task AddNotificationAsync(string receiverId, string type, string? postId = null);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync();
        Task MarkAsReadAsync(int id);
    }
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationService(
            ApplicationDbContext context,
            IHubContext<NotificationHub> hubContext,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _hubContext = hubContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        private string? GetCurrentUserId() =>
            _userManager.GetUserId(_httpContextAccessor.HttpContext?.User);

        public async Task AddNotificationAsync(string receiverId, string type, string? postId = null)
        {
            var senderId = GetCurrentUserId();
            if (senderId == null) return;
            if (senderId == receiverId) return;

            var message = type switch
            {
                "Like" => "liked your post ❤️",
                "Follow" => "followed you 👤",
                "Reply" => "replied to your tweet 💬",
                "Retweet" => "retweeted your tweet 🔁",
                _ => "sent you a notification"
            };

            var notification = new Notification
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Type = type,
                PostId = postId,
                Message = message
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group(receiverId).SendAsync("ReceiveNotification", new
            {
                notification.Id,
                notification.Type,
                notification.Message,
                notification.PostId,
                notification.CreatedAt
            });
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Enumerable.Empty<Notification>();

            return await _context.Notifications
                .Where(n => n.ReceiverId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return;

            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
}