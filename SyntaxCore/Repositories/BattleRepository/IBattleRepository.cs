using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Constants;

namespace SyntaxCore.Repositories.BattleRepository
{
    public interface IBattleRepository
    {
        /// <summary>
        /// Creates a new battle record in the database.
        /// </summary>
        /// <param name="battle"></param>
        public Task<Battle> CreateBattle(Battle battle);

        /// <summary>
        /// Extracts all available battles based on the provided filters.
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="minQuestionsCount"></param>
        /// <param name="maxQuestionsCount"></param>
        public Task<List<Battle>> GetAllAvailableBattles(
            List<string>? categories,
            int? minQuestionsCount = null,
            int? maxQuestionsCount = null
        );
    }
}
