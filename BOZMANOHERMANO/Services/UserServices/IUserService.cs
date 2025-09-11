using StartUp.Dtos;
using StartUp.HiddenServices;
using StartUp.Repo;

namespace StartUp.Services.UserServices
{
    public interface IUserService
    {
        UserDto GetUserCredentials();
        List<UserDto> SearchForUser(string searchTerm);

        void EditUserCredentials(EditUserDto dto, IFormFile profile, IFormFile header);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepo _repo;
        private readonly IUserContext _userContext;
        private readonly IFileService _fileService;

        public UserService(
            IUserRepo repo,
            IUserContext userContext,
            IFileService fileService
            )
        {
            _repo = repo;
            _userContext = userContext;
            _fileService = fileService;
        }
        public UserDto GetUserCredentials()
        {
            var userIdFromToken = _userContext.GetUserId();

            var user = _repo.GetUserCredentials(userIdFromToken);
            if (user == null)
                return null;

            return new UserDto
            {
                UserName = user.UserName,
                TagName = user.TagName,
                ProfilePicPath = user.ProfilePicPath,
                HeaderPath = user.HeaderPath,
                Bio = user.Bio
            };
        }
        public void EditUserCredentials(EditUserDto dto, IFormFile profile, IFormFile header)
        {
            var userIdFromToken = _userContext.GetUserId();

            var user = _repo.GetUserCredentials(userIdFromToken);
            if (user == null) return;

            var ProfilePath = _fileService.UploadPPAsync(profile, "Uploads/PPs");
            var HeaderPath = _fileService.UploadPPAsync(header, "Uploads/Heads");

            user.UserName = dto.UserName ?? user.UserName;
            user.TagName = dto.TagName ?? user.TagName;
            user.ProfilePicPath = ProfilePath ?? user.ProfilePicPath;
            user.HeaderPath = HeaderPath ?? user.HeaderPath;
            user.Bio = dto.Bio ?? user.Bio;

            _repo.EditUserCredentials(user);
        }

        public List<UserDto> SearchForUser(string searchTerm)
        {
            var user = _repo.SearchForUser(searchTerm);
            return user.Select(u => new UserDto
            {
                UserName = u.UserName,
                TagName = u.TagName,
                ProfilePicPath = u.ProfilePicPath,
                HeaderPath = u.HeaderPath,
                Bio = u.Bio
            }).ToList();
        }
    }
}
