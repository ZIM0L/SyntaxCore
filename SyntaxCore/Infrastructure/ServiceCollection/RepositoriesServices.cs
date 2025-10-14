using SyntaxCore.Repositories.UserRepository;

namespace SyntaxCore.Infrastructure.ServiceCollection
{
    public static class RepositoriesServices
    {
        public static IServiceCollection AddRepositoriesServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
