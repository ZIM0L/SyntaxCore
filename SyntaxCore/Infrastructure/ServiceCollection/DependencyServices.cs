using DotNetEnv;
using StackExchange.Redis;

namespace SyntaxCore.Infrastructure.ServiceCollection
{
    public static class DependencyServices
    {
        public static IServiceCollection AddDependencyService(this IServiceCollection services, IConfigurationManager configuration, string? license = "")
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyServices).Assembly);
                cfg.LicenseKey = license;
            });
            if (configuration.GetValue<string>("ConnectionStrings:RedisConnection") is string connection)
            {
                services.AddSingleton<IConnectionMultiplexer>(sp =>
                    ConnectionMultiplexer.Connect(connection.Normalize())
                );
            }
            return services;
        }
    }
   
}