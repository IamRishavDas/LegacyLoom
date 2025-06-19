using MassTransit;
using NotificationService.EventConsumers;

namespace NotificationService.ConsumerRegistrationExtension
{
    public static class MassTransitConsumerConfigurationRegistration
    {
        public static void AddMassTransitRegistrationForConsumer(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserRegisteredConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("user-registered-queue", e =>
                    {
                        e.ConfigureConsumer<UserRegisteredConsumer>(context);
                    });
                });
            });
        }
    }
}
