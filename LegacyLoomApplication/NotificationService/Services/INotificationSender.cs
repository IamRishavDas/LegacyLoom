
using ServiceResponseShared;

namespace NotificationService.Services
{
    public interface INotificationSender
    {
        Task<ServiceResponse<string>> SendWelcomeNotificationAsync(string toEmail, string userName);
    }
}