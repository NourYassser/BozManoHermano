using StartUp.Dtos;
using StartUp.HiddenServices;
using StartUp.Repo;

namespace StartUp.Services.UserServices
{
    public interface IUserService
    {
        UserDto GetUserCredentials();

        void EditUserCredentials(EditUserDto dto, IFormFile file);
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
        public void EditUserCredentials(EditUserDto dto, IFormFile file)
        {
            var userIdFromToken = _userContext.GetUserId();

            var user = _repo.GetUserCredentials(userIdFromToken);
            if (user == null) return;

            var filePath = _fileService.UploadPPAsync(file, "Uploads/PPs");


            user.UserName = dto.UserName;
            user.TagName = dto.TagName;
            user.ProfilePicPath = filePath;
            user.HeaderPath = dto.HeaderPath;
            user.Bio = dto.Bio;

            _repo.EditUserCredentials(user);
        }
    }
}
