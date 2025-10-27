using MediatR;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Repositories.BattleParticipantRepository;

namespace SyntaxCore.Application.GameSession.Commands.AssignPlayersToBattle
{
    public class CreateNewBattleParticipant(IBattleParticipantRepository battleParticipantRepository) : IRequestHandler<CreateNewBattleParticipantRequest, BattleParticipant>
    {
        public async Task<BattleParticipant> Handle(CreateNewBattleParticipantRequest request, CancellationToken cancellationToken)
        {
            var battleParticipant = new BattleParticipant
            {
                UserFK = request.PlayerId,
                BattleFK = request.BattleId,
                Score = 0
            };

            await battleParticipantRepository.AddBattleParticipantAsync(battleParticipant);

            // add error handling as needed


            return battleParticipant;
        }
    }
}
