using MassTransit;
using NotificationService.EventConsumers;

namespace NotificationService.ConsumerRegistrationExtension
{
    public static class MassTransitConsumerConfigurationRegistration
    {
        public static void AddMassTransitRegistrationForConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserRegisteredConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["MessabeBrokerHost:Host"] ?? throw new ArgumentNullException("Message broker host not found"));

                    cfg.ReceiveEndpoint("user-registered-queue", e =>
                    {
                        e.ConfigureConsumer<UserRegisteredConsumer>(context);
                    });
                });
            });
        }
    }
}
