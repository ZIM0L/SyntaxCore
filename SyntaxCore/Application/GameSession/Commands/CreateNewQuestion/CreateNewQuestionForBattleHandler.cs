using MediatR;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Repositories.QuestionOptionRepository;
using SyntaxCore.Repositories.QuestionRepository;

namespace SyntaxCore.Application.GameSession.Commands.CreateNewQuestions
{
    public class CreateNewQuestionForBattleHandler(IQuestionOptionRepository questionOptionRepository, IQuestionRepository questionRepository) : IRequestHandler<CreateNewQuestionForBattleRequest, Unit>
    {
        public async Task<Unit> Handle(CreateNewQuestionForBattleRequest request, CancellationToken cancellationToken)
        {
            var question = new Question
            {
                QuestionText = request.questionText,
                Category = request.category,
                Difficulty = request.difficulty,
                TimeForAnswerInSeconds = request.timeForAnswerInSeconds,
                Explanation = request.explanation,
            };

            if (question.TimeForAnswerInSeconds <= 20)
            {
                throw new QuestionCreationException("Time for one a question cannot be less then 20 seconds");
            }

            await questionRepository.createQuestion(question);

            var QuestionOptions = new List<QuestionOption>();

            request.correctAnswers.ForEach(answer =>
            {
               QuestionOptions.Add(new QuestionOption
                {
                   QuestionFK = question.QuestionId,
                   OptionAnswer = answer,
                   IsCorrect = true
                });
            });

            request.wrongAnswers.ForEach(answer =>
            {
                QuestionOptions.Add(new QuestionOption
                {
                    QuestionFK = question.QuestionId,
                    OptionAnswer = answer,
                    IsCorrect = false
                });
            });

            await questionOptionRepository.createQuestionOption(QuestionOptions);

            return Unit.Value;
        }
    }
}
