using BOZMANOHERMANO.Services.UserFollowServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BOZMANOHERMANO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FollowController : ControllerBase
    {
        private readonly IUserFollowService _userFollowService;

        public FollowController(IUserFollowService userFollowService)
        {
            _userFollowService = userFollowService;
        }

        [HttpGet("GetFollowers")]
        public IActionResult CheckFollowers(string userName)
        {
            return Ok(_userFollowService.GetUserFollowers(userName));
        }

        [HttpGet("GetFollowing")]
        public IActionResult CheckFollowing(string userName)
        {
            return Ok(_userFollowService.GetUserFollowing(userName));
        }

        [HttpPost("Follow")]
        public IActionResult Follow(string FollowedId)
        {
            var x = _userFollowService.Follow(FollowedId);
            return Ok(x);
        }
    }
}
