using BOZMANOHERMANO.Dtos;
using BOZMANOHERMANO.Models;
using BOZMANOHERMANO.Repo;
using StartUp.Dtos;
using StartUp.HiddenServices;

namespace BOZMANOHERMANO.Services.UserFollowServices
{
    public interface IUserFollowService
    {
        IEnumerable<UserDto> GetUserFollowers(string userName);
        IEnumerable<UserDto> GetUserFollowing(string userName);
        int GetUserFollowersCount(string userName);

        void Follow(UserFollowDto userFollow);
        void UnFollow(string followedId);
    }
    public class UserFollowService : IUserFollowService
    {
        private readonly IUserFollow _repo;
        private readonly IUserContext _userContext;

        public UserFollowService(IUserFollow repo, IUserContext userContext)
        {
            _repo = repo;
            _userContext = userContext;
        }
        public IEnumerable<UserDto> GetUserFollowers(string userName)
        {
            var user = _repo.GetUserFollowers(userName).ToList()
                .Select(
                p => new UserDto()
                {
                    UserName = p.Follower.UserName,
                    TagName = p.Follower.TagName,
                    ProfilePicPath = p.Follower.ProfilePicPath,
                    HeaderPath = p.Follower.HeaderPath,
                    Bio = p.Follower.Bio
                });

            return user;
        }
        public IEnumerable<UserDto> GetUserFollowing(string userName)
        {
            var user = _repo.GetUserFollowing(userName).ToList()
                .Select(
                p => new UserDto()
                {
                    UserName = p.Followed.UserName,
                    TagName = p.Followed.TagName,
                    ProfilePicPath = p.Followed.ProfilePicPath,
                    HeaderPath = p.Followed.HeaderPath,
                    Bio = p.Followed.Bio
                });

            return user;
        }

        public int GetUserFollowersCount(string userName)
        {
            return _repo.GetUserFollowersCount(userName);
        }

        public void Follow(UserFollowDto userFollow)
        {
            var userID = _userContext.GetUserId();

            var follow = new UserFollow()
            {
                FollowedId = userFollow.FollowedId,
                FollowerId = userID,
                FollowedDate = DateTime.UtcNow.Date
            };
            _repo.Follow(follow);
        }

        public void UnFollow(string followedId)
        {
            var userID = _userContext.GetUserId();

            _repo.UnFollow(userID, followedId);
        }

    }
}
