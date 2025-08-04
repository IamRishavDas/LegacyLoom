using EventModelsShared;
using MassTransit;
using NotificationService.EmailTemplates;
using NotificationService.Models;
using NotificationService.Services;

namespace NotificationService.EventConsumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegistered>
    {
        private readonly INotificationSender _notificationSender;
        private readonly INotificationService _notficationService;

        public UserRegisteredConsumer(INotificationSender notificationSender, INotificationService notficationService)
        {
            _notificationSender = notificationSender;
            _notficationService = notficationService;
        }

        public async Task Consume(ConsumeContext<UserRegistered> context)
        {
            try
            {
                var message = context.Message;
                var response = await _notificationSender.SendWelcomeNotificationAsync(message.Email, message.Username);
                if (response.Success)
                {
                    Console.WriteLine($"Email sended successfully to: {message.Email}");
                    var notificationCreateResponse = await _notficationService.Create(new Notification()
                    {
                        SendToUserId = message.Id.ToString(),
                        SendToUserEmail = message.Email,
                        TemplateUsed = TemplateName.WELCOME.ToString(),
                    });
                    if (!notificationCreateResponse.Success) throw new Exception("Error while storing the notification log to the Notification Collection");
                }
                else
                {
                    Console.WriteLine($"There was some problem while sending email: {message.Email}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not able to send the message, Exception: {ex.Message}");
            }
        }
    }
}
