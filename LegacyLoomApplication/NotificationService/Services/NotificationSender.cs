using MailKit.Net.Smtp;
using MimeKit;
using NotificationService.EmailTemplates;
using ServiceResponseShared;
using System.Net;

namespace NotificationService.Services
{
    public class NotificationSender : INotificationSender
    {
        private readonly Templates _templates;

        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _senderEmail;
        private readonly string _appPassword;
        private readonly string _senderName;

        public NotificationSender(Templates templates, IConfiguration configuration)
        {
            _templates = templates;

            _smtpServer = configuration["EmailSettings:SmtpServer"]   ?? throw new ArgumentNullException("Smtp Server information not found!");
            _port = int.Parse(configuration["EmailSettings:Port"]     ?? throw new ArgumentNullException("Port information not found!"));
            _senderEmail = configuration["EmailSettings:SenderEmail"] ?? throw new ArgumentNullException("Sender Email information not found!");
            _appPassword = configuration["EmailSettings:AppPassword"] ?? throw new ArgumentNullException("App Password information not found!");
            _senderName = configuration["EmailSettings:SenderName"]   ?? throw new ArgumentNullException("Sender Name information not found!");
        }

        
        public async Task<ServiceResponse<string>> SendWelcomeNotificationAsync(string toEmail, string userName)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_senderName, _senderEmail));
                message.To.Add(new MailboxAddress(userName, toEmail));
                message.Subject = "Welcome to Legacy Loom";

                var bodyBuilder = new BodyBuilder()
                {
                    HtmlBody = _templates.GetTemplate(TemplateName.WELCOME, userName)
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(_senderEmail, _appPassword);

                var result = await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return ServiceResponse<string>.SuccessResult(result, (int)HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                return ServiceResponse<string>.Failure("Error while sending the welcome notification", ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
