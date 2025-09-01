using Microsoft.EntityFrameworkCore;
using StartUp.Models;
using StartUp.Models.Data;

namespace BOZMANOHERMANO.Repo
{
    public interface IPostsRepo
    {
        List<string> Following(string userId);
        List<Posts> GetPosts(string username);
        List<Posts> GetFollowingPosts(List<string> followingList);

        void Post(Posts post);
    }
    public class PostsRepo : IPostsRepo
    {
        private readonly ApplicationDbContext _context;
        public PostsRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<string> Following(string userId)
        {
            return _context.UserFollows
                .Where(uf => uf.FollowerId == userId)
                .Select(uf => uf.FollowedId)
                .ToList();
        }

        public List<Posts> GetPosts(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) return new List<Posts>();
            return _context.Posts
                .Where(p => p.UserId == user.Id)
                .Include(List => List.CommentList)
                .Include(username => username.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }
        public List<Posts> GetFollowingPosts(List<string> followingList)
        {
            return _context.Posts
                .Where(p => followingList.Contains(p.UserId))
                .Include(List => List.CommentList)
                .Include(username => username.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }
        public void Post(Posts post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }
    }
}
