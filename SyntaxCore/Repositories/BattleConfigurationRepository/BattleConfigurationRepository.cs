using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.DbContext;

namespace SyntaxCore.Repositories.BattleConfigurationRepository
{
    public class BattleConfigurationRepository : BaseRepository, IBattleConfigurationRepository
    {
        public BattleConfigurationRepository(MyDbContext context) : base(context) { }

        public async Task CreateBattleConfigurationAsync(List<BattleConfiguration> battleConfiguration)
        {
            await _context.BattleConfigurations.AddRangeAsync(battleConfiguration);
        }
    }
}
