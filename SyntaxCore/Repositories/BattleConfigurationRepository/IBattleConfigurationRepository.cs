using MediatR;
using SyntaxCore.Entities.BattleRelated;

namespace SyntaxCore.Repositories.BattleConfigurationRepository
{
    public interface IBattleConfigurationRepository
    {
        /// <summary>
        /// creates new battle configuration records in the database.
        /// </summary>
        public Task CreateBattleConfigurationAsync(List<BattleConfiguration> battleConfiguration);

        /// <summary>
        /// Deletes battle configurations by battle id.
        /// </summary>
        public Task DeleteBattleConfigurations(Guid battleId);
    }
}
