using MediatR;
using SyntaxCore.Models.BattleRelated;
using SyntaxCore.Repositories.BattleRepository;

namespace SyntaxCore.Application.GameSession.Queries
{
    public class GetAllAvailableBattles(IBattleRepository battleRepository) : IRequestHandler<GetAllAvailableBattlesRequest, List<BattleDto>>
    {
        public async Task<List<BattleDto>> Handle(GetAllAvailableBattlesRequest request, CancellationToken cancellationToken)
        {
            var battles = await battleRepository.GetAllAvailableBattles(
                categories: request.Categories,
                minQuestionsCount: request.MinQuestionsCount,
                maxQuestionsCount: request.MaxQuestionsCount
            );
            var battleDtos = battles.Select(battle => new BattleDto
            {
                BattleId = battle.BattleId,
                BattleName = battle.BattleName,
                Category = battle.Category,
                QuestionsCount = battle.QuestionsCount,
                PlayerId1 = "tempPlayer1",
                PlayerId2 = "tempPlayer2",
                Status = battle.Status,
                CreatedAt = battle.CreatedAt,
                EndedAt = battle.EndedAt
            }).ToList();
            return battleDtos;
        }
    }
}
