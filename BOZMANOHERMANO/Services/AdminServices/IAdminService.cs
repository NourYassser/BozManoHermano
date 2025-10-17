using BOZMANOHERMANO.Dtos;
using Microsoft.AspNetCore.Identity;
using StartUp.Models;
using StartUp.Models.Data;

namespace BOZMANOHERMANO.Services.AdminServices
{
    public interface IAdminService
    {
        Task<string> VerifyUserAsync(string userId);
        Task<string> BanUserAsync(string userId);
        Task<string> UnbanUserAsync(string userId);

        string DeletePost(int postId);

        DashboardStatsDto GetDashboardStats();
    }
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminService(UserManager<ApplicationUser> userManager,
                            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        public DashboardStatsDto GetDashboardStats()
        {
            var today = DateTime.UtcNow.Date;

            return new DashboardStatsDto
            {
                TotalUsers = _context.Users.Count(),
                VerifiedUsers = _context.Users.Count(u => u.IsVerified),
                BannedUsers = _context.Users.Count(u => u.IsBanned),
                TotalPosts = _context.Posts.Count(),
                PostsToday = _context.Posts.Count(p => p.CreatedAt.Date == today)
            };
        }

        public async Task<string> VerifyUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found";

            user.IsVerified = true;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return $"{user.UserName} is now verified ✅";
        }

        public async Task<string> BanUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found";

            user.IsBanned = true;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return $"{user.UserName} has been banned 🚫";
        }

        public async Task<string> UnbanUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found";

            user.IsBanned = false;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return $"{user.UserName} has been unbanned ✅";
        }

        public string DeletePost(int postId)
        {
            var post = _context.Posts.Find(postId);
            if (post == null) return "Post not found";

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return "Post deleted successfully 🗑️";
        }
    }
}