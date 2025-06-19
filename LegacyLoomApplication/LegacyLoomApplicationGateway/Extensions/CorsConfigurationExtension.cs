namespace LegacyLoomApplicationGateway.Extensions
{
    public static class CorsConfigurationExtension
    {
        public static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            var policyName = configuration["Cors:Policy"] ?? throw new ArgumentNullException("Cors:Policy not found!");
            var origins = configuration["Cors:Domains"] ?? throw new ArgumentNullException("Cors:Domains not found!");
            services.AddCors(options =>
            {
                options.AddPolicy(name: policyName,
                                  policy =>
                                  {
                                      policy.WithOrigins(origins)
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                                  });
            });
        }
    }
}
