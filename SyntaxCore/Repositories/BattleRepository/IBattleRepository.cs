using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Models.BattleRelated;

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
        public Task<List<BattleDto>> GetAllAvailableBattlesWithFilters(
            string? BattleName,
            List<string>? Categories,
            int? DifficultyLevel,
            int? MinQuestionsCount,
            int? MaxQuestionsCount);
        /// <summary>
        /// Fetches a battle by its public identifier.
        /// </summary>
        /// <param name="battlePublicId"></param>
        public Task<Battle?> GetBattleByPublicId(Guid battlePublicId);
        /// <summary>
        /// Updates the status of a battle.
        /// </summary>
        Task UpdateBattleStatus(Guid battleId, string status);
        /// <summary>
        /// Deletes a battle by its identifier.
        /// </summary>
        /// <param name="battleId"></param>
        public Task DeleteBattle(Guid battleId);
        /// <summary>
        /// gets all games with waiting status
        /// </summary>
        /// <returns></returns>
        public Task<List<Battle>?> GetAllGamesWithWaitingStatusAsync(string? battleName);
        /// <summary>
        /// Fetches all questions associated with a specific battle.
        /// <summary/>
        Task<List<QuestionForBattleDto>> FetchAllQuestionsForBattle(Guid battlePublicId);
    }
}
