using SyntaxCore.Interfaces;

namespace SyntaxCore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {       
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            return services;
        }
    }
}
