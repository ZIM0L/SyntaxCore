using MediatR;
using SyntaxCore.Models.BattleRelated;
using System.Diagnostics;

namespace SyntaxCore.Application.GameSession.Queries.FetchQuestionsForBattle
{
    public class FetchQuestionsForBattleHandler : IRequestHandler<FetchQuestionsForBattleRequest, QuestionForBattleDto>
    {
    }
}
