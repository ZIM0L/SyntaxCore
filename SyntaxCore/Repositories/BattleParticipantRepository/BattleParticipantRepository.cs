using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.DbContext;

namespace SyntaxCore.Repositories.BattleParticipantRepository
{
    public class BattleParticipantRepository : BaseRepository , IBattleParticipantRepository
    {
        public BattleParticipantRepository(MyDbContext context) : base(context) { }
        public async Task AddBattleParticipantAsync(BattleParticipant participant)
        {
            await _context.BattleParticipants.AddAsync(participant);
            await _context.SaveChangesAsync();
        }
    }
}
