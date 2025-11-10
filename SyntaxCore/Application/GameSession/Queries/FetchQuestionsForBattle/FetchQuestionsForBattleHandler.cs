using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Models.BattleRelated;
using SyntaxCore.Repositories.BattleRepository;
using SyntaxCore.Repositories.QuestionOptionRepository;
using SyntaxCore.Repositories.QuestionRepository;
using System.Diagnostics;
using System.Text.Json;

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
            var cachedQuestions = await distributedCache.GetAsync(request.BattlePublicId.ToString());
            if (cachedQuestions is not null)
            {
                await distributedCache.RefreshAsync(request.BattlePublicId.ToString());
                return  JsonSerializer.Deserialize<List<QuestionForBattleDto>>(cachedQuestions) ?? throw new ArgumentException("Battle couldn't start");
            }

            var FetchedQuestionsFromDB = await battleRepository.FetchAllQuestionsForBattle(request.BattlePublicId) ?? throw new ArgumentException("Battle couldn't start");

            await distributedCache.SetAsync(
                request.BattlePublicId.ToString(),
                JsonSerializer.SerializeToUtf8Bytes(FetchedQuestionsFromDB),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            return FetchedQuestionsFromDB;
        }
    }
}
