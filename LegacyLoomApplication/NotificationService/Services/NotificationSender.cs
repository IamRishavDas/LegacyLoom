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
        private readonly string API_KEY;
        private readonly string EMAIL;
        private readonly string APPLICAITON_NAME;
        public NotificationSender(Templates templates, IConfiguration configuration)
        {
            _templates = templates;
            API_KEY = configuration["SendGridEmailSettings:ApiKey"] ?? throw new ArgumentNullException("SendGrid: ApiKey not found");
            EMAIL = configuration["SendGridEmailSettings:Email"] ?? throw new ArgumentNullException("SendGrid: Email not found");
            APPLICAITON_NAME = configuration["SendGridEmailSettings:ApplicationName"] ?? throw new ArgumentNullException("SendGrid: ApplicationName not found");
        }

        public async Task<ServiceResponse<Response>> SendWelcomeNotificationAsync(string toEmail, string userName)
        {
            try
            {
                var client = new SendGridClient(API_KEY);
                var from = new EmailAddress(EMAIL, APPLICAITON_NAME);
                var to = new EmailAddress(toEmail, userName);
                var msg = MailHelper.CreateSingleEmail(from, to, "Welcome to Legacy Loom", "", _templates.GetTemplate(TemplateName.WELCOME, userName));
                var response = await client.SendEmailAsync(msg);
                if (!response.IsSuccessStatusCode) throw new Exception();
                return ServiceResponse<Response>.SuccessResult(response, (int)HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                return ServiceResponse<Response>.Failure("Error while sending the welcome notification", ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
