using SyntaxCore.Entities.BattleRelated;

namespace SyntaxCore.Repositories.BattleParticipantRepository
{
    public interface IBattleParticipantRepository 
    {
        public Task AddBattleParticipantAsync(BattleParticipant participant);
    }
}
