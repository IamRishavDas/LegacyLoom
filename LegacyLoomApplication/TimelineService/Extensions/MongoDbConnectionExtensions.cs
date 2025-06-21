using MongoDB.Driver;
using TimelineService.Settings;

namespace TimelineService.Extensions
{
    public static class MongoDbConnectionExtensions
    {
        public static void LoadMongoDbSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TimelineDbSettings>(
                configuration.GetSection("TimelineDatabaseSettings")
            );
        }

        public static void CreateMongoClientInstance(this IServiceCollection services, IConfiguration configuration)
        {
             services.AddSingleton<IMongoClient>(_ => {
                var connectionString =
                    configuration
                        .GetSection("TimelineDatabaseSettings:ConnectionString")?
                        .Value;

                return new MongoClient(connectionString);
            });
        }
    }
}
