using MediatR;

namespace SyntaxCore.Application.GameSession.Commands.DeleteBattle
{
    public record DeleteBattleRequest(
        Guid UserId,
        Guid battlePublicId
        ) : IRequest<Unit>;
}
