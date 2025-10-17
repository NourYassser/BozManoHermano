using BOZMANOHERMANO.Services.AdminServices;
using Microsoft.AspNetCore.Mvc;

namespace BOZMANOHERMANO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard-stats")]
        public IActionResult GetDashboardStats()
        {
            var stats = _adminService.GetDashboardStats();
            return Ok(stats);
        }

        [HttpPost("verify-user/{userId}")]
        public async Task<IActionResult> VerifyUser(string userId)
        {
            var result = await _adminService.VerifyUserAsync(userId);
            return Ok(result);
        }

        [HttpPost("ban-user/{userId}")]
        public async Task<IActionResult> BanUser(string userId)
        {
            var result = await _adminService.BanUserAsync(userId);
            return Ok(result);
        }

        [HttpPost("unban-user/{userId}")]
        public async Task<IActionResult> UnbanUser(string userId)
        {
            var result = await _adminService.UnbanUserAsync(userId);
            return Ok(result);
        }

        [HttpDelete("delete-post/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            var result = _adminService.DeletePost(postId);
            return Ok(result);
        }
    }
}
