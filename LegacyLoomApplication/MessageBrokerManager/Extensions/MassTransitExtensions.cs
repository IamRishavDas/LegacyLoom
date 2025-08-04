
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBrokerManager.Extensions
{
    public static class MassTransitExtensions
    {
        public static void AddMassTransitConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["MessabeBrokerHost:Host"] ?? throw new ArgumentNullException("Message broker host not found"));
                });
            });
        }
    }
}
