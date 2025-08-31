using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StartUp.Dtos;
using StartUp.Services.UserServices;

namespace StartUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserCredentialsController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserCredentialsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetCredentials")]
        public IActionResult GetUserCredentials()
        {
            var userCredentials = _userService.GetUserCredentials();

            if (userCredentials == null)
                return NotFound("User not found");
            return Ok(userCredentials);
        }

        [HttpPatch("EditCredentials")]
        public IActionResult EditUserCredentials([FromForm] EditUserDto dto)
        {
            _userService.EditUserCredentials(dto, dto.ProfilePicPath, dto.HeaderPath);
            return Ok("User credentials updated successfully");
        }
    }
}
