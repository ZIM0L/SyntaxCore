using MediatR;
using SyntaxCore.Entities.BattleRelated;

namespace SyntaxCore.Application.GameSession.Commands.JoinGameSession
{
    public record JoinBattleRequest
    (
        Guid UserId,
        Guid BattlePublicId
    ) : IRequest<Unit>;
}
