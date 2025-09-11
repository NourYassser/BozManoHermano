using StartUp.Models;
using StartUp.Models.Data;

namespace StartUp.Repo
{
    public interface IUserRepo
    {
        ApplicationUser GetUserCredentials(string userId);
        List<ApplicationUser> SearchForUser(string searchTerm, int pageNum = 1, int pageSize = 10);

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

        public List<ApplicationUser> SearchForUser(string searchTerm, int pageNum = 1, int pageSize = 10)
        {
            var query = _context.ApplicationUsers.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.UserName.Contains(searchTerm) || u.Email.Contains(searchTerm));
            }
            return query
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
