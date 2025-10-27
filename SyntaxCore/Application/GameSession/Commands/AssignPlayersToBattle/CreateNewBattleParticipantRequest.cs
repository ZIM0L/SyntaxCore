using MediatR;
using SyntaxCore.Entities.BattleRelated;

namespace SyntaxCore.Application.GameSession.Commands.AssignPlayersToBattle
{
    public record CreateNewBattleParticipantRequest(
    Guid PlayerId,
    Guid BattleId,
    string Role 
    ) : IRequest<BattleParticipant>;
}
