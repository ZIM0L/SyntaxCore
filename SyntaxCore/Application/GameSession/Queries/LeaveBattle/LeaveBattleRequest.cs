using MediatR;

namespace SyntaxCore.Application.GameSession.Queries.LeaveBattle
{
    public record LeaveBattleRequest(
        Guid PublicBattleId,
        Guid UserId
        ): IRequest<Unit>;
}
