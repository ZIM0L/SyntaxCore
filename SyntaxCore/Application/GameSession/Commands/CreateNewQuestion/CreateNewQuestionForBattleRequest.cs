using MediatR;

namespace SyntaxCore.Application.GameSession.Commands.CreateNewQuestions
{
    public record CreateNewQuestionForBattleRequest
    (
       string questionText,
       string category, 
       List<string> correctAnswers,
       List<string> wrongAnswers,
       int difficulty,
       int timeForAnswerInSeconds,
       string? explanation
    ) : IRequest<Unit>;
}