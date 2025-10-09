using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace StartUp.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, AlternateView htmlView, CancellationToken cancellationToken = default);
        Task SendWelcomeEmailAsync(string to, CancellationToken cancellationToken = default);
        Task<AlternateView> BuildWelcomeHtmlAsync(string userName);

        Task ForgetPassword(string to, string resetLink, CancellationToken cancellationToken = default);
    }

    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config, ILogger<EmailSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        private (string Host, int Port, bool EnableSsl, string From, string Password) GetSmtpSettings()
        {
            var host = _config["Smtp:Host"] ?? "smtp.gmail.com";
            var port = int.TryParse(_config["Smtp:Port"], out var p) ? p : 587;
            var ssl = bool.TryParse(_config["Smtp:EnableSsl"], out var s) ? s : true;
            var from = _config["Smtp:From"] ?? throw new InvalidOperationException("Smtp:From not configured");
            var pass = _config["Smtp:Password"] ?? throw new InvalidOperationException("Smtp:Password not configured");

            return (host, port, ssl, from, pass);
        }

        public async Task SendEmailAsync(string to, string subject, AlternateView htmlView, CancellationToken cancellationToken = default)
        {
            var (host, port, enableSsl, from, password) = GetSmtpSettings();

            using var message = new MailMessage();
            message.From = new MailAddress(from);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.AlternateViews.Add(htmlView);

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 20000
            };

            try
            {
                _logger.LogInformation("Sending email to {To}", to);
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                await client.SendMailAsync(message).ConfigureAwait(false);
                _logger.LogInformation("Email sent to {To}", to);
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "SMTP error when sending email to {To}: {Message}", to, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when sending email to {To}", to);
                throw;
            }
        }

        public async Task<AlternateView> BuildWelcomeHtmlAsync(string userName)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/material/html", "welcome_template.html");
            var html = await File.ReadAllTextAsync(templatePath);

            html = html.Replace("{{UserName}}", userName);

            var htmlView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);

            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/material/img/1660708609305.jpg");
            var logo = new LinkedResource(logoPath, MediaTypeNames.Image.Jpeg)
            {
                ContentId = "logoImage",
                TransferEncoding = TransferEncoding.Base64
            };
            htmlView.LinkedResources.Add(logo);

            // Embed hero image
            var heroPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/material/img/1726138347782.jpg");
            var hero = new LinkedResource(heroPath, MediaTypeNames.Image.Jpeg)
            {
                ContentId = "heroImage",
                TransferEncoding = TransferEncoding.Base64
            };

            htmlView.LinkedResources.Add(hero);

            return htmlView;
        }

        public Task SendWelcomeEmailAsync(string to, CancellationToken cancellationToken = default)
        {
            var html = BuildWelcomeHtmlAsync("صديق").Result;
            return SendEmailAsync(to, "🎉Welcome to Boz Mano Hermano", html, cancellationToken);
        }

        public async Task ForgetPassword(string to, string resetLink, CancellationToken cancellationToken = default)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/material/html", "reset_password_template.html");
            var html = await File.ReadAllTextAsync(templatePath);
            html = html.Replace("{{ResetLink}}", resetLink);
            var htmlView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/material/img/1660708609305.jpg");
            var logo = new LinkedResource(logoPath, MediaTypeNames.Image.Jpeg)
            {
                ContentId = "logoImage",
                TransferEncoding = TransferEncoding.Base64
            };
            htmlView.LinkedResources.Add(logo);
            await SendEmailAsync(to, "🔒 Password Reset Request", htmlView, cancellationToken);
        }
    }
}
