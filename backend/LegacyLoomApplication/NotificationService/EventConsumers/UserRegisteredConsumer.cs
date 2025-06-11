using EventModelsShared;
using MassTransit;

namespace NotificationService.EventConsumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegistered>
    {
        public Task Consume(ConsumeContext<UserRegistered> context)
        {
            var message = context.Message;
            Console.WriteLine($"Username: {message.Username}, Email: {message.Email}");
            return Task.CompletedTask;
        }
    }
}
