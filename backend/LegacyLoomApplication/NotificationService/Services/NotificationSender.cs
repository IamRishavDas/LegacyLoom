using NotificationService.EmailTemplates;
using SendGrid;
using SendGrid.Helpers.Mail;
using ServiceResponseShared;
using System.Net;

namespace NotificationService.Services
{
    public class NotificationSender : INotificationSender
    {
        private readonly Templates _templates;
        private readonly IConfiguration _configuration;
        public NotificationSender(Templates templates, IConfiguration configuration)
        {
            _templates = templates;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<Response>> SendNotification(string toEmail, string subject, string userName, string message, TemplateName template)
        {
            try
            {
                var client = new SendGridClient(_configuration["SendGridEmailSettings:ApiKey"]);
                var from = new EmailAddress(_configuration["SendGridEmailSettings:Email"], _configuration["SendGridEmailSettings:ApplicationName"]);
                var to = new EmailAddress(toEmail, userName);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, message, _templates.GetTemplate(template, userName));
                var response = await client.SendEmailAsync(msg);
                return ServiceResponse<Response>.SuccessResult(response, (int)HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                return ServiceResponse<Response>.Failure("Error while sending the welcome notification", ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
