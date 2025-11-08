using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public async Task DeleteBattleConfigurations(Guid battleId)
        {
            var configurations = _context.BattleConfigurations.Where(cfg => cfg.BattleFK == battleId);
            _context.BattleConfigurations.RemoveRange(configurations);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BattleConfiguration>?> getBattlesConfigurationsAsync(List<Battle> battles)
        {
            return await _context.BattleConfigurations
                .Where(cfg => battles.Select(b => b.BattleId).Contains(cfg.BattleFK))
                .ToListAsync();
        }
    }
}
