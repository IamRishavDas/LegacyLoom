using EventModelsShared;
using MassTransit;
using NotificationService.EmailTemplates;
using NotificationService.Services;

namespace NotificationService.EventConsumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegistered>
    {
        private readonly INotificationSender _notificationSender;

        public UserRegisteredConsumer(INotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }

        public async Task Consume(ConsumeContext<UserRegistered> context)
        {
            try
            {
                var message = context.Message;
                //Console.WriteLine($"UserId: {message.Id}, Username: {message.Username}, Email: {message.Email}");
                await _notificationSender
                    .SendNotification(
                        message.Email,
                        "Welcome to Legacy Loom",
                        message.Username,
                        "",
                        TemplateName.WELCOME
                    );
                Console.WriteLine($"Email sended successfully to: {message.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not able to send the message, Exception: {ex.Message}");
            }
        }
    }
}
