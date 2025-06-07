using Ocelot.DependencyInjection;
using Ocelot.Cache.CacheManager;

namespace LegacyLoomApplicationGateway.Extensions
{
    public static class OcelotJsonConfigurationExtension
    {

        public static void AddJsonFilesForOcelotConfig(this IConfigurationBuilder configuration)
        {
            configuration
                .AddJsonFile("Configurations/globalConfig.json", optional: false, reloadOnChange: true)
                .AddJsonFile("Configurations/authService.json",  optional: false, reloadOnChange: true)
                .AddJsonFile("Configurations/userService.json",  optional: false, reloadOnChange: true);
        }

        public static void AddOcelotConfig(this IServiceCollection services)
        {
            services.AddOcelot()
                .AddCacheManager(options =>
                {
                    options.WithDictionaryHandle();
                }
            );
        }
    }
}
