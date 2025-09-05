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

        string Follow(string FollowedId);
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

        public string Follow(string FollowedId)
        {
            var userID = _userContext.GetUserId();

            var follow = new UserFollow()
            {
                FollowedId = FollowedId,
                FollowerId = userID,
                FollowedDate = DateTime.UtcNow.Date
            };
            return _repo.Follow(follow);
        }

    }
}
