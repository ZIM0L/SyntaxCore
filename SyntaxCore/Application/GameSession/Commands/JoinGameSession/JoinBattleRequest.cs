using MediatR;
using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Models.BattleRelated;

namespace SyntaxCore.Application.GameSession.Commands.JoinGameSession
{
    public record JoinBattleRequest
    (
        Guid UserId,
        Guid BattlePublicId,
        string Role 
    ) : IRequest<BattleParticipantsDto>;
}
