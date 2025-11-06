using MediatR;
using Microsoft.Extensions.Caching.Distributed;
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
        IDistributedCache distributedCache) : IRequestHandler<FetchQuestionsForBattleRequest, QuestionForBattleDto>
    {
        public async Task<QuestionForBattleDto> Handle(FetchQuestionsForBattleRequest request, CancellationToken cancellationToken)
        {
            var question = new QuestionForBattleDto();

            var fetchedQuestion = await questionRepository.GetRandomQuestionByCategoryAndDifficulty(request.category, request.difficulty, request.questionCountToGet);

            return question;
        }
    }
}
