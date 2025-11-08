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
        IBattleRepository battleRepository,
        IBattleConfigurationRepository battleConfigurationRepository
        ) : IRequestHandler<GetAllAvailableBattlesRequest, List<BattleDto>>
    {
        public async Task<List<BattleDto>> Handle(GetAllAvailableBattlesRequest request, CancellationToken cancellationToken)
        {
            if (await battleRepository.GetAllGamesWithWaitingStatusAsync() is not List<Battle> battlesToFetch)
            {
                throw new BattleException("No battles with waiting status found.");
            }

            await battleConfigurationRepository.getBattlesConfigurationsAsync();

            return await battleRepository.GetAllAvailableBattlesWithFilters(
                request.BattleName,
                request.Categories,
                request.DifficultyLevel,
                request.MinQuestionsCount,
                request.MaxQuestionsCount);
        }
    }
}
