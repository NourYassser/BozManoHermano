using Microsoft.AspNetCore.Mvc;
using StartUp.Services;

namespace BOZMANOHERMANO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IEmailSender emailSender,
            ILogger<EmailController> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            try
            {
                var html = await _emailSender.BuildWelcomeHtmlAsync(User.Identity.Name);
                await _emailSender.SendEmailAsync(email, "🎉 Welcome to Boz Mano Hermano", html, cancellationToken);
                return Ok(new { message = "Email sent." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", email);
                return StatusCode(500, "Failed to send email.");
            }
        }

        /*[HttpPost("SendResetPasswordEmail")]
        public async Task<IActionResult> SendResetPasswordEmail(string email, string resetLink, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(resetLink))
                return BadRequest("Email and reset link are required.");
            try
            {
                await _emailSender.ForgetPassword(email, resetLink, cancellationToken);
                return Ok(new { message = "Password reset email sent." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
                return StatusCode(500, "Failed to send password reset email.");
            }
        }*/

        /*[HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = WebUtility.UrlEncode(token);

            var resetLink = $"{email}&token={encodedToken}";

            var htmlView = await _emailSender.ForgetPassword(user.UserName, resetLink);

            await _emailSender.SendEmailAsync(email, "Reset Your Password", htmlView);

            return Ok("Password reset email sent.");
        }*/

    }
}
