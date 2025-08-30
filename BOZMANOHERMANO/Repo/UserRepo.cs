using StartUp.Models;
using StartUp.Models.Data;

namespace StartUp.Repo
{
    public interface IUserRepo
    {
        ApplicationUser GetUserCredentials(string userId);

        void EditUserCredentials(ApplicationUser User);
    }
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public ApplicationUser GetUserCredentials(string userId)
        {
            return _context.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
        }
        public void EditUserCredentials(ApplicationUser User)
        {
            _context.ApplicationUsers.Update(User);
            _context.SaveChanges();
        }

    }
}
