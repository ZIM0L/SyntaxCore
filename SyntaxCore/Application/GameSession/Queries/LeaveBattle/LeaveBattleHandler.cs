using MediatR;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Repositories.BattleParticipantRepository;
using SyntaxCore.Repositories.BattleRepository;

namespace SyntaxCore.Application.GameSession.Queries.LeaveBattle
{
    public class LeaveBattleHandler(
        IBattleParticipantRepository battleParticipantRepository,
        IBattleRepository battleRepository
        ) : IRequestHandler<LeaveBattleRequest, Unit>
    {
        public async Task<Unit> Handle(LeaveBattleRequest request, CancellationToken cancellationToken)
        {
            var battle = await battleRepository.GetBattleByPublicId(request.PublicBattleId) ?? throw new BattleException("Battlee not found");

            var battleParticipants = await battleParticipantRepository.GetParticipantsByBattleId(battle.BattleId) ?? throw new BattleException("Battle not found");

            if (!battleParticipants.Any(p => p.UserFK == request.UserId))
            {
                throw new BattleException("User is not a participant of the battle");
            }
            
            await battleParticipantRepository.RemoveParticipantFromBattle(request.PublicBattleId, request.UserId); 
            
            return Unit.Value;
        }
    }
}
