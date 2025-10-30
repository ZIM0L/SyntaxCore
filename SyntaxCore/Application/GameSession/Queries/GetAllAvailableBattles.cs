using MediatR;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Models.BattleRelated;
using SyntaxCore.Repositories.BattleParticipantRepository;
using SyntaxCore.Repositories.BattleRepository;
using System.Linq;

namespace SyntaxCore.Application.GameSession.Queries
{
    public class GetAllAvailableBattles(IBattleRepository battleRepository, IBattleParticipantRepository battleParticipantRepository) : IRequestHandler<GetAllAvailableBattlesRequest, List<BattleDto>>
    {
        public async Task<List<BattleDto>> Handle(GetAllAvailableBattlesRequest request, CancellationToken cancellationToken)
        {
            var battles = await battleRepository.GetAllAvailableBattles(
                categories: request.Categories,
                minQuestionsCount: request.MinQuestionsCount,
                maxQuestionsCount: request.MaxQuestionsCount
            );

            var battleIds = battles.Select(b => b.BattlePublicId).ToList();

            var participants = await battleParticipantRepository.GetParticipantsByBattleIds(battleIds);

            var battleDtos = battles.GroupJoin(
                participants,
                battle => battle.BattlePublicId,
                participant => participant.BattleFK,
                (battle, battleParticipants) => new BattleDto
                {
                    BattleId = battle.BattlePublicId,
                    BattleName = battle.BattleName,
                    Category = battle.Category,
                    QuestionsCount = battle.QuestionsCount,
                    PlayerId1 = battleParticipants.ElementAtOrDefault(0)?.User.Username ?? string.Empty,
                    PlayerId2 = battleParticipants.ElementAtOrDefault(1)?.User.Username ?? string.Empty,
                    Status = battle.Status,
                    CreatedAt = battle.CreatedAt,
                    EndedAt = battle.EndedAt
                }
                ).ToList();
            return battleDtos;
        }
    }
}
