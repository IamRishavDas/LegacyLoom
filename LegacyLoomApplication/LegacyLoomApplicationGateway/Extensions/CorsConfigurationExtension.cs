namespace LegacyLoomApplicationGateway.Extensions
{
    public static class CorsConfigurationExtension
    {
        public static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            var policyName = configuration["Cors:Policy"] ?? throw new ArgumentNullException("Cors:Policy not found!");
            var origins = configuration["Cors:Domains"] ?? throw new ArgumentNullException("Cors:Domains not found!");
            var tauriOrigin = configuration["Cors:Tauri"] ?? throw new ArgumentNullException("Cors:Tauri domain not found!");
            var tauriProdOrigin = configuration["Cors:TauriProd"] ?? throw new ArgumentNullException("Cors:Tauri Prod domain not found!");
            var tauriProdMacOrigin = configuration["Cors:TauriProdMac"] ?? throw new ArgumentNullException("Cors:Tauri Prod Mac domain not found!");

            services.AddCors(options =>
            {
                options.AddPolicy(name: policyName,
                                  policy =>
                                  {
                                      policy.WithOrigins([origins, tauriOrigin, tauriProdOrigin, tauriProdMacOrigin])
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .WithExposedHeaders("X-Pagination", "Authorization");
                                  });
            });
        }
    }
}
