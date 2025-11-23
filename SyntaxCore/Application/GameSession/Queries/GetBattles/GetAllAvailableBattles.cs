using MediatR;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Models.BattleRelated;
using SyntaxCore.Repositories.BattleConfigurationRepository;
using SyntaxCore.Repositories.BattleParticipantRepository;
using SyntaxCore.Repositories.BattleRepository;
using System.Linq;

namespace SyntaxCore.Application.GameSession.Queries
{
    public class GetAllAvailableBattles(
        IBattleRepository battleRepository
        ) : IRequestHandler<GetAllAvailableBattlesRequest, List<BattleDto>>
    {
        public async Task<List<BattleDto>> Handle(GetAllAvailableBattlesRequest request, CancellationToken cancellationToken)
        {
            var battles = await battleRepository.GetAllAvailableBattlesWithFilters(
                request.BattleName,
                request.Categories,
                request.DifficultyLevel,
                request.MinQuestionsCount,
                request.MaxQuestionsCount);

            if (request.MinQuestionsCount.HasValue)
            {
                battles.RemoveAll(b => b.TotalQuestionsCount < request.MinQuestionsCount.Value);
            }

            if (request.MaxQuestionsCount.HasValue)
            {
                battles.RemoveAll(b => b.TotalQuestionsCount > request.MaxQuestionsCount.Value);
            }

            return battles;

        }
    }
}
