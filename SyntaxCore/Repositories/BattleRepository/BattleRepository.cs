using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.DbContext;

namespace SyntaxCore.Repositories.BattleRepository
{
    public class BattleRepository : BaseRepository, IBattleRepository
    {
        public BattleRepository(MyDbContext context) : base(context)
        {
            
        }

        public async Task<Battle> CreateBattle(Battle battle)
        {
            await _context.Battles.AddAsync(battle);
            await _context.SaveChangesAsync();
            return battle;
        }
    }
}
