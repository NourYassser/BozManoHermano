using BOZMANOHERMANO.Models;
using Microsoft.EntityFrameworkCore;
using StartUp.Models.Data;

namespace BOZMANOHERMANO.Repo
{
    public interface IUserDmRepo
    {
        List<UserDM> GetMessages(string userId);
        List<UserDM> OpenChat(string senderId, string recId);

        string SendMessage(UserDM userDM);
    }

    public class UserDmRepo : IUserDmRepo
    {
        private readonly ApplicationDbContext _context;

        public UserDmRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UserDM> GetMessages(string userId)
        {
            var lastMessageIds = _context.UserDM
                .Where(m => m.SenderId == userId || m.RecieverId == userId)
                .GroupBy(m => m.SenderId == userId ? m.RecieverId : m.SenderId)
                .Select(g => g.OrderByDescending(m => m.MessageDate)
                              .Select(m => m.Id)
                              .FirstOrDefault())
                .ToList();

            if (!lastMessageIds.Any())
                return new List<UserDM>();

            var lastMessages = _context.UserDM
                .Where(m => lastMessageIds.Contains(m.Id))
                .Include(m => m.Sender)
                .Include(m => m.Reciever)
                .OrderByDescending(m => m.MessageDate)
                .AsNoTracking()
                .ToList();

            return lastMessages;
        }


        public List<UserDM> OpenChat(string senderId, string recId)
        {
            return _context.UserDM
                .Where(p => p.SenderId == senderId && p.RecieverId == recId)
                .Include(p => p.Sender)
                .AsNoTracking()
                .OrderByDescending(p => p.MessageDate)
                .ToList();
        }

        public string SendMessage(UserDM userDM)
        {
            _context.UserDM.Add(userDM);
            _context.SaveChanges();

            var username = _context.ApplicationUsers
                .Where(p => p.Id == userDM.RecieverId)
                .Select(p => p.UserName)
                .FirstOrDefault();

            return $"Message sent to {username}";
        }
    }
}
