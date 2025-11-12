using MediatR;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Repositories.BattleParticipantRepository;
using SyntaxCore.Repositories.BattleRepository;

namespace SyntaxCore.Application.GameSession.Queries.FetchPlayersParticipantsGuids
{
    public class FetchPlayersParticipantsGuidsHandler(
        IBattleParticipantRepository battleParticipantRepository,
        IBattleRepository battleRepository
        ) : IRequestHandler<FetchPlayersParticipantsGuidsRequest, List<Guid>>
    {
        public async Task<List<Guid>> Handle(FetchPlayersParticipantsGuidsRequest request, CancellationToken cancellationToken)
        {
            var battle = await battleRepository.GetBattleByPublicId(request.PublicBattleId) ?? throw new BattleException("Battle not found");
            var participants = await battleParticipantRepository.GetParticipantsByBattleId(battle.BattleId) ?? throw new BattleException("No participants found for the given battle");

            return participants.Select(p => p.UserFK).ToList();
        }
    }
}
