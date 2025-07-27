using CloudinaryDotNet;

namespace TimelineService.Extensions
{
    public static class CloudinaryConfigExtensions
    {
        public static void AddCloudinaryServiceAsSingleton(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudinary = new Cloudinary(new Account(
                   configuration["Cloudinary:CloudName"],
                   configuration["Cloudinary:ApiKey"],
                   configuration["Cloudinary:ApiSecret"]
                ));
            services.AddSingleton(cloudinary);
        }
    }
}
