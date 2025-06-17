
using NotificationService.EmailTemplates;
using SendGrid;
using ServiceResponseShared;

namespace NotificationService.Services
{
    public interface INotificationSender
    {
        Task<ServiceResponse<Response>> SendWelcomeNotificationAsync(string toEmail, string userName);
    }
}