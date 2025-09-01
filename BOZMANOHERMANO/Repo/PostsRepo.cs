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
        void DeletePost(string userId, int id);

        void Comment(Comments comments);
        void DeleteComment(string userId, int id);
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
                .Include(p => p.User)
                .Include(p => p.CommentList)
                     .ThenInclude(c => c.User)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToList();
        }
        public List<Posts> GetFollowingPosts(List<string> followingList)
        {
            return _context.Posts
                .Where(p => followingList.Contains(p.UserId))
                .Include(p => p.User)
                .Include(List => List.CommentList)
                    .ThenInclude(c => c.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }
        public void Post(Posts post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void DeletePost(string userId, int id)
        {
            var ent = _context.Posts.FirstOrDefault(p => p.Id == id && p.UserId == userId);
            if (ent != null)
                _context.Posts.Remove(ent);
            _context.SaveChanges();
        }

        public void Comment(Comments comments)
        {
            _context.Comments.Add(comments);
            _context.SaveChanges();
        }
        public void DeleteComment(string userId, int id)
        {
            var ent = _context.Comments.FirstOrDefault(p => p.Id == id && p.UserId == userId);
            if (ent != null)
                _context.Comments.Remove(ent);
            _context.SaveChanges();
        }
    }
}
