using SyntaxCore.Repositories.BattleParticipantRepository;
using SyntaxCore.Repositories.BattleRepository;
using SyntaxCore.Repositories.QuestionOptionRepository;
using SyntaxCore.Repositories.QuestionRepository;
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
            services.AddScoped<IQuestionOptionRepository, QuestionOptionRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            return services;
        }
    }
}
