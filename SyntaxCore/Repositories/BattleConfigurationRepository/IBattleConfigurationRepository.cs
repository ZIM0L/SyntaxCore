using SyntaxCore.Entities.BattleRelated;

namespace SyntaxCore.Repositories.BattleConfigurationRepository
{
    public interface IBattleConfigurationRepository
    {
        /// <summary>
        /// creates new battle configuration records in the database.
        /// </summary>
        public Task CreateBattleConfigurationAsync(List<BattleConfiguration> battleConfiguration);
    }
}
