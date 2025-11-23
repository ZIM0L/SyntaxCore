using MediatR;
using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Repositories.BattleConfigurationRepository;
using SyntaxCore.Repositories.BattleParticipantRepository;
using SyntaxCore.Repositories.BattleRepository;

namespace SyntaxCore.Application.GameSession.Commands.DeleteBattle
{
    public class DeleteBattleHandler(
        IBattleRepository battleRepository,
        IBattleConfigurationRepository battleConfigurationRepository,
        IBattleParticipantRepository battleParticipantRepository
        ) : IRequestHandler<DeleteBattleRequest, Unit>
    {
        public async Task<Unit> Handle(DeleteBattleRequest request, CancellationToken cancellationToken)
        {
            var dbBattle = await battleRepository.GetBattleByPublicId(request.battlePublicId) ?? throw new BattleException("Battle not found");

            //later add admin check
            if (dbBattle.BattleOwnerFK != request.UserId){
                throw new BattleException("Only the battle owner can delete the battle");
            }
            if(dbBattle.Status == BattleStatuses.InProgress)
            {
                throw new BattleException("Cannot delete a battle that is in progress");
            }

            await battleConfigurationRepository.DeleteBattleConfigurations(dbBattle.BattleId);
            await battleParticipantRepository.DeleteBattleParticipants(dbBattle.BattleId);

            await battleRepository.DeleteBattle(dbBattle.BattleId);

            return Unit.Value;
        }
    }
}
