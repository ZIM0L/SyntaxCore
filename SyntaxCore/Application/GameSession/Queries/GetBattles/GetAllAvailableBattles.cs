using MediatR;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Models.BattleRelated;
using SyntaxCore.Repositories.BattleParticipantRepository;
using SyntaxCore.Repositories.BattleRepository;
using System.Linq;

namespace SyntaxCore.Application.GameSession.Queries
{
    public class GetAllAvailableBattles(IBattleRepository battleRepository) : IRequestHandler<GetAllAvailableBattlesRequest, List<BattleDto>>
    {
        public async Task<List<BattleDto>> Handle(GetAllAvailableBattlesRequest request, CancellationToken cancellationToken)
        {
            return await battleRepository.GetAllAvailableBattlesWithFilters(
                request.BattleName,
                request.Categories,
                request.DifficultyLevel,
                request.MinQuestionsCount,
                request.MaxQuestionsCount);
        }
    }
}
