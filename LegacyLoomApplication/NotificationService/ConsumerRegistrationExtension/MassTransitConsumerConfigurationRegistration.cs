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
                    cfg.Host(configuration["MessabeBrokerHost:Host"] ?? throw new ArgumentNullException("Message broker host not found"), h =>
                    {
                        h.UseSsl(s =>
                        {
                            s.Protocol = System.Security.Authentication.SslProtocols.Tls12;
                        });
                        h.Heartbeat(TimeSpan.FromSeconds(120));
                    });

                    cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60)));
                    cfg.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(5)));

                    cfg.ReceiveEndpoint("user-registered-queue", e =>
                    {
                        e.Durable = true;
                        e.AutoDelete = false;
                        e.ConfigureConsumer<UserRegisteredConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(10)));
                    });
                });
            });
        }
    }
}
