
using NotificationService.EmailTemplates;
using SendGrid;
using ServiceResponseShared;

namespace NotificationService.Services
{
    public interface INotificationSender
    {
        Task<ServiceResponse<Response>> SendNotification(string toEmail, string subject, string userName, string message, TemplateName template);
    }
}