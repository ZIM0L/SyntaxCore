using Microsoft.EntityFrameworkCore;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.DbContext;

namespace SyntaxCore.Repositories.BattleParticipantRepository
{
    public class BattleParticipantRepository : BaseRepository, IBattleParticipantRepository
    {
        public BattleParticipantRepository(MyDbContext context) : base(context) { }
        public async Task AddBattleParticipantAsync(BattleParticipant participant)
        {
            await _context.BattleParticipants.AddAsync(participant);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BattleParticipant>> GetParticipantsByBattles(List<Battle> battles)
        {
           return await _context.BattleParticipants
                .Where(bp => battles.Select(b => b.BattleId).Contains(bp.BattleFK))
                .Include(bp => bp.User)
                .ToListAsync();
        }

        public async Task<List<BattleParticipant>?> GetParticipantsByBattleId(Guid battleid)
        {
            return await _context.BattleParticipants
                .Where(bp => bp.BattleFK == battleid)
                .ToListAsync();
        }

        public async Task DeleteBattleParticipants(Guid battleId)
        {
            var participants = _context.BattleParticipants.Where(bp => bp.BattleFK == battleId);
            _context.BattleParticipants.RemoveRange(participants);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveParticipantFromBattle(Guid battlePublicId, Guid playerId)
        {
            await _context.BattleParticipants
               .Where(bp => bp.Battle.BattlePublicId == battlePublicId && bp.UserFK == playerId)
               .ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
    }
}
