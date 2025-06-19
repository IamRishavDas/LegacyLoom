
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBrokerManager.Extensions
{
    public static class MassTransitExtensions
    {
        public static void AddMassTransitConfigurations(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });
        }
    }
}
