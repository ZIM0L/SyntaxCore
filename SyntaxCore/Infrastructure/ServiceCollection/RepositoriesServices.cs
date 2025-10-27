using SyntaxCore.Repositories.BattleParticipantRepository;
using SyntaxCore.Repositories.BattleRepository;
using SyntaxCore.Repositories.UserRepository;

namespace SyntaxCore.Infrastructure.ServiceCollection
{
    public static class RepositoriesServices
    {
        public static IServiceCollection AddRepositoriesServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBattleRepository, BattleRepository>();
            services.AddScoped<IBattleParticipantRepository, BattleParticipantRepository>();

            return services;
        }
    }
}
