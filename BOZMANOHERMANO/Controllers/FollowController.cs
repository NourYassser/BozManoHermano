using BOZMANOHERMANO.Services.Notifications;
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
        private readonly INotificationService _notificationService;

        public FollowController(IUserFollowService userFollowService,
            INotificationService notificationService)
        {
            _userFollowService = userFollowService;
            _notificationService = notificationService;
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
        public async Task<IActionResult> Follow(string FollowedId)
        {
            var x = _userFollowService.Follow(FollowedId);

            await _notificationService.AddNotificationAsync(FollowedId, "Follow");

            return Ok(x);
        }
    }
}
