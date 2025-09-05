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

        string Follow(UserFollow userFollow);
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

        public string Follow(UserFollow userFollow)
        {
            if (userFollow.FollowerId == userFollow.FollowedId)
                return "You can't follow yourself, dummy XD";

            var toBeRemoved = _context.UserFollows
                .FirstOrDefault(p => p.FollowedId == userFollow.FollowedId
                && p.FollowerId == userFollow.FollowerId);

            var userName = _context.ApplicationUsers
                .Where(p => p.Id == userFollow.FollowedId)
                .Select(u => u.UserName)
                .FirstOrDefault();

            if (toBeRemoved != null)
            {
                _context.UserFollows.Remove(toBeRemoved);
                _context.SaveChanges();

                return $"You have unfollowed, {userName}";
            }
            _context.UserFollows.Add(userFollow);
            _context.SaveChanges();

            return $"You have followed, {userName}";
        }

    }
}
