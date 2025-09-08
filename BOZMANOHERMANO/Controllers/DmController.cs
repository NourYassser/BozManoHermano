using BOZMANOHERMANO.Services.DmServices;
using Microsoft.AspNetCore.Mvc;

namespace BOZMANOHERMANO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DmController : ControllerBase
    {
        private readonly IUserDmService _userDmService;

        public DmController(IUserDmService userDmService)
        {
            _userDmService = userDmService;
        }

        [HttpGet("GetDm")]
        public IActionResult GetDm()
        {
            return Ok(_userDmService.GetUserMessages());
        }

        [HttpGet("OpenChat")]
        public IActionResult OpenChat(string senderId)
        {
            return Ok(_userDmService.OpenChat(senderId));
        }

        [HttpPost("SendMessage")]
        public IActionResult SendMessage(string message, string recId)
        {
            var mess = _userDmService.SendMessage(message, recId);
            return Ok(mess);
        }

    }
}
