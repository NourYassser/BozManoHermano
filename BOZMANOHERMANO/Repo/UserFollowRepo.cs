using BOZMANOHERMANO.Models;
using Microsoft.EntityFrameworkCore;
using StartUp.Models.Data;

namespace BOZMANOHERMANO.Repo
{
    public interface IUserFollow
    {
        List<UserFollow> GetUserFollowers(string userId);
        List<UserFollow> GetUserFollowing(string userName);
        int GetUserFollowersCount(string userId);

        void Follow(UserFollow userFollow);
        void UnFollow(string userId, string followerId);
    }

    public class UserFollowRepo : IUserFollow
    {
        private readonly ApplicationDbContext _context;

        public UserFollowRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<UserFollow> GetUserFollowers(string userName)
        {
            return _context.UserFollows
                .Include(p => p.Follower)
                .Where(p => p.Followed.UserName == userName)
                .ToList();
        }
        public List<UserFollow> GetUserFollowing(string userName)
        {
            return _context.UserFollows
                .Include(p => p.Followed)
                .Where(p => p.Follower.UserName == userName)
                .ToList();
        }

        public int GetUserFollowersCount(string userId)
        {
            var count = _context.UserFollows.Where(p => p.FollowedId == userId).ToList();
            return count.Count;
        }

        public void Follow(UserFollow userFollow)
        {
            _context.UserFollows.Add(userFollow);
            _context.SaveChanges();
        }
        public void UnFollow(string userId, string followerId)
        {
            var toBeRemoved = _context.UserFollows
                .FirstOrDefault(p => p.FollowedId == followerId
                && p.FollowerId == userId);

            _context.UserFollows.Remove(toBeRemoved);
            _context.SaveChanges();
        }

    }
}
