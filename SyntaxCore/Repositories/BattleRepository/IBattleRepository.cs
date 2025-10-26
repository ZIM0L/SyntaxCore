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
        /// <returns></returns>
        public Task<Battle> CreateBattle(Battle battle);
    }
}
