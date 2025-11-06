using Microsoft.EntityFrameworkCore;
using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.DbContext;

namespace SyntaxCore.Repositories.BattleRepository
{
    public class BattleRepository : BaseRepository, IBattleRepository
    {
        public BattleRepository(MyDbContext context) : base(context) { }

        public async Task<Battle> CreateBattle(Battle battle)
        {
            await _context.Battles.AddAsync(battle);
            await _context.SaveChangesAsync();
            return battle;
        }
        public async Task<List<Battle>> GetAllAvailableBattles(List<string>? categories, int? minQuestionsCount = null, int? maxQuestionsCount = null)
        {
            var query = _context.Battles.AsQueryable();

            query = query.Where(b => b.Status == BattleStatuses.Waiting);

            //if (categories != null && categories.Count > 0)
            //    query = query.Where(b => categories.Contains(b.Category));

            if (minQuestionsCount.HasValue)
                query = query.Where(b => b.QuestionsCount >= minQuestionsCount.Value);

            if (maxQuestionsCount.HasValue)
                query = query.Where(b => b.QuestionsCount <= maxQuestionsCount.Value);

            return await query.ToListAsync();
        }

        public async Task<Battle?> GetBattleByPublicId(Guid battlePublicId)
        {
            return await _context.Battles.FirstOrDefaultAsync(b => b.BattlePublicId == battlePublicId);
        }

        public async Task UpdateBattleStatus(Guid battlePublicId, string status)
        {
            await _context.Battles
                .Where(b => b.BattlePublicId == battlePublicId)
                .ExecuteUpdateAsync(b => b.SetProperty(b => b.Status, status));
        }
    }
}
