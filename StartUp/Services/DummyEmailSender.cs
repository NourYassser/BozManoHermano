using Microsoft.AspNetCore.Identity;
using StartUp.Models;

namespace StartUp.Services
{
    public class DummyEmailSender : IEmailSender<ApplicationUser>
    {
        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            Console.WriteLine($"[CONFIRMATION LINK] {confirmationLink}");
            return Task.CompletedTask;
        }

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            Console.WriteLine($"[RESET LINK] {resetLink}");
            return Task.CompletedTask;
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            Console.WriteLine($"[RESET CODE] {resetCode}");
            return Task.CompletedTask;
        }
    }
}
