using BOZMANOHERMANO.Models;
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

        string Post(Posts post);
        string DeletePost(string userId, int id);

        List<Likes> PostLikes(int postid);
        string Like(Likes likes);

        List<Retweets> PostRetweets(int postid);
        string Retweet(Retweets retweets);

        string Comment(Comments comments);
        string DeleteComment(string userId, int id);

        string Save(SavedPosts savedPosts);
        List<SavedPosts> GetSavedPosts(string userId);
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

        public string Post(Posts post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();

            return "Post Added";
        }

        public string DeletePost(string userId, int id)
        {
            var ent = _context.Posts.FirstOrDefault(p => p.Id == id && p.UserId == userId);
            if (ent != null)
                _context.Posts.Remove(ent);
            _context.SaveChanges();
            return "Post Deleted";
        }
        #endregion

        #region SavedPosts
        public string Save(SavedPosts savedPosts)
        {
            var existingSave = _context.SavedPosts
                .FirstOrDefault(sp => sp.UserId == savedPosts.UserId
                                                    && sp.PostId == savedPosts.PostId);
            var x = "";
            if (existingSave != null)
            {
                _context.SavedPosts.Remove(existingSave);
                x = "Post Unsaved";
            }
            else
            {
                _context.SavedPosts.Add(savedPosts);
                x = "Post Saved";
            }
            _context.SaveChanges();
            return x;
        }
        public List<SavedPosts> GetSavedPosts(string userId)
        {
            return _context.SavedPosts
                .Where(sp => sp.UserId == userId)
                .Include(sp => sp.User)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.User)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.LikesList)
                        .ThenInclude(c => c.User)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.RetweetsList)
                         .ThenInclude(c => c.User)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.CommentList)
                        .ThenInclude(c => c.User)
                .OrderByDescending(sp => sp.SaveDate)
                .ToList();
        }
        #endregion

        #region Comments
        public string Comment(Comments comments)
        {
            _context.Comments.Add(comments);
            _context.SaveChanges();
            return "Comment Added";
        }
        public string DeleteComment(string userId, int id)
        {
            var ent = _context.Comments.FirstOrDefault(p => p.Id == id && p.UserId == userId);
            if (ent != null)
                _context.Comments.Remove(ent);
            _context.SaveChanges();
            return "Comment Deleted";
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

        public string Like(Likes likes)
        {
            var existingLike = _context.Likes
                .FirstOrDefault(l => l.UserId == likes.UserId
                                                    && l.PostId == likes.PostId);
            var x = "";
            if (existingLike != null)
            {
                _context.Likes.Remove(existingLike);
                x = "You unliked this post";
            }
            else
            {
                _context.Likes.Add(likes);
                x = "You liked this post";
            }

            _context.SaveChanges();
            return x;
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

        public string Retweet(Retweets retweets)
        {
            var existingLike = _context.Retweets
                .FirstOrDefault(l => l.UserId == retweets.UserId
                                                    && l.PostId == retweets.PostId);
            var x = "";
            if (existingLike != null)
            {
                _context.Retweets.Remove(existingLike);
                x = "You unretweeted this post";
            }
            else
            {
                _context.Retweets.Add(retweets);
                x = "You retweeted this post";
            }

            _context.SaveChanges();
            return x;
        }
        #endregion
    }
}
