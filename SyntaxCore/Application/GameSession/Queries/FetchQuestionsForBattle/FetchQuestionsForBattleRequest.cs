using MediatR;
using SyntaxCore.Models.BattleRelated;

namespace SyntaxCore.Application.GameSession.Queries.FetchQuestionsForBattle
{
    public record FetchQuestionsForBattleRequest
    (
        Guid BattlePublicId
    ) : IRequest<QuestionForBattleDto>;
}
