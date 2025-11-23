using MediatR;

namespace SyntaxCore.Application.GameSession.Queries.FetchPlayersParticipantsGuids
{
    public record FetchPlayersParticipantsGuidsRequest
    (
        Guid PublicBattleId
    ) : IRequest<List<Guid>>;
}
