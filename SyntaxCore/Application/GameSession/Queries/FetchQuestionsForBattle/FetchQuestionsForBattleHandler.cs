using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Models.BattleRelated;
using SyntaxCore.Repositories.BattleRepository;
using SyntaxCore.Repositories.QuestionOptionRepository;
using SyntaxCore.Repositories.QuestionRepository;
using System.Diagnostics;

namespace SyntaxCore.Application.GameSession.Queries.FetchQuestionsForBattle
{
    public class FetchQuestionsForBattleHandler(
        IQuestionRepository questionRepository,
        IQuestionOptionRepository questionOptionRepository,
        IBattleRepository battleRepository,
        IDistributedCache distributedCache) : IRequestHandler<FetchQuestionsForBattleRequest, List<QuestionForBattleDto>>
    {
        public async Task<List<QuestionForBattleDto>> Handle(FetchQuestionsForBattleRequest request, CancellationToken cancellationToken)
        {
            var question = new List<QuestionForBattleDto>();

            Console.WriteLine("Fetching questions for battle with ID: " + request.BattlePublicId);
            var battle = await battleRepository.FetchAllQuestionsForBattle(request.BattlePublicId) ?? throw new ArgumentException("Battle couldn't start");


            //if (await questionRepository.GetRandomQuestionByCategoryAndDifficulty(battle.Category, battle.diff, request.questionCountToGet)
            //    is not List<Question> fetchedQuestion)
            //{
            //    throw new QuestionNotAvailableException("No questions matching the given category or difficulty were found in the database.");
            //}

            //if (fetchedQuestion.Count < request.questionCountToGet)
            //{
            //    throw new QuestionNotAvailableException("Not enough questions available to fulfill the requested amount.");
            //}

            //Dictionary<Guid, List<QuestionOption>> dictionaryWithQuestions = new Dictionary<Guid, List<QuestionOption>>();

            //fetchedQuestion.ForEach(async qo =>
            //{
            //    dictionaryWithQuestions[qo.QuestionId] = await questionOptionRepository.GetQuestionOptionsByQuestionId(qo.QuestionId) 
            //        ?? throw new QuestionNotAvailableException("unknown error occured during fetching question");
            //});

            return question;
        }
    }
}
