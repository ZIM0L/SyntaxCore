using DotNetEnv;

namespace SyntaxCore.Infrastructure.ServiceCollection
{
    public static class DependencyServices
    {
        public static IServiceCollection AddDependencyService(this IServiceCollection services, string? license = "")
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyServices).Assembly);
                cfg.LicenseKey = license;
            });
            return services;
        }
    }
   
}