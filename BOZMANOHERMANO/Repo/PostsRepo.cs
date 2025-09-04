using Microsoft.EntityFrameworkCore;
using StartUp.Models;
using StartUp.Models.Data;

namespace BOZMANOHERMANO.Repo
{
    public interface IPostsRepo
    {
        List<string> Following(string userId);
        List<string> Followers(string userId);

        List<Posts> GetPosts(string username);
        List<Posts> GetFollowingPosts(List<string> followingList);

        void Post(Posts post);
        void DeletePost(string userId, int id);

        List<Likes> PostLikes(int postid);
        void Like(Likes likes);

        List<Retweets> PostRetweets(int postid);
        void Retweet(Retweets retweets);

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

        //Followings: 216
        public List<string> Following(string userId)
        {
            return _context.UserFollows
                .Where(uf => uf.FollowerId == userId)
                .Select(uf => uf.FollowedId)
                .ToList();
        }

        //Followers: 226
        public List<string> Followers(string userId)
        {
            return _context.UserFollows
               .Where(uf => uf.FollowedId == userId)
               .Select(uf => uf.FollowerId)
               .ToList();
        }

        #region Post
        public List<Posts> GetPosts(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) return new List<Posts>();
            return _context.Posts
                .Where(p => p.UserId == user.Id || p.RetweetsList.Any(r => r.UserId == user.Id))
                .Include(p => p.User)
                .Include(p => p.LikesList)
                    .ThenInclude(c => c.User)
                .Include(p => p.CommentList)
                    .ThenInclude(c => c.User)
                .Include(p => p.RetweetsList)
                     .ThenInclude(c => c.User)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToList();
        }
        public List<Posts> GetFollowingPosts(List<string> followingList)
        {
            return _context.Posts
                .Where(p => followingList.Contains(p.UserId) ||
                    p.RetweetsList.Any(r => followingList.Contains(r.UserId)))
                .Include(p => p.User)
                .Include(p => p.LikesList)
                    .ThenInclude(c => c.User)
                .Include(p => p.CommentList)
                    .ThenInclude(c => c.User)
                .Include(p => p.RetweetsList)
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
        #endregion

        #region Comments
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
        #endregion

        #region Likes
        public List<Likes> PostLikes(int postid)
        {
            return _context.Likes
                .FirstOrDefault(l => l.PostId == postid) == null ? new List<Likes>() : _context.Likes
                .Include(l => l.User)
                .ToList();
        }

        public void Like(Likes likes)
        {
            var existingLike = _context.Likes
                .FirstOrDefault(l => l.UserId == likes.UserId
                                                    && l.PostId == likes.PostId);

            if (existingLike != null)
            {
                _context.Likes.Remove(existingLike);
            }
            else
            {
                _context.Likes.Add(likes);
            }

            _context.SaveChanges();
        }

        #endregion

        #region Retweets
        public List<Retweets> PostRetweets(int postid)
        {
            return _context.Retweets
                .FirstOrDefault(l => l.PostId == postid) == null ? new List<Retweets>() : _context.Retweets
                .Include(l => l.User)
                .ToList();
        }

        public void Retweet(Retweets retweets)
        {
            var existingLike = _context.Retweets
                .FirstOrDefault(l => l.UserId == retweets.UserId
                                                    && l.PostId == retweets.PostId);

            if (existingLike != null)
            {
                _context.Retweets.Remove(existingLike);
            }
            else
            {
                _context.Retweets.Add(retweets);
            }

            _context.SaveChanges();
        }
        #endregion
    }
}
