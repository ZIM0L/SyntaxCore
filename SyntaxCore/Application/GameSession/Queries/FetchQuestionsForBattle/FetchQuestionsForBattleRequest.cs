using MediatR;
using SyntaxCore.Models.BattleRelated;

namespace SyntaxCore.Application.GameSession.Queries.FetchQuestionsForBattle
{
    public record FetchQuestionsForBattleRequest
    (
       string category,
       int difficulty,
       int? timeForAnswerInSeconds,
       int questionCountToGet
    ) : IRequest<QuestionForBattleDto>;
}
