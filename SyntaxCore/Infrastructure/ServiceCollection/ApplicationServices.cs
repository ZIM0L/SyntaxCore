using SyntaxCore.Infrastructure.Implementations;
using SyntaxCore.Interfaces;

namespace SyntaxCore.Infrastructure.ServiceCollection
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {       
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            return services;
        }
    }
}
