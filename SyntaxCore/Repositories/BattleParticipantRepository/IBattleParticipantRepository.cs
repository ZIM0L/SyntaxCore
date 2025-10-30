using SyntaxCore.Entities.BattleRelated;

namespace SyntaxCore.Repositories.BattleParticipantRepository
{
    public interface IBattleParticipantRepository 
    {
        /// <summary>
        /// Creates a new battle participant record in the database.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        public Task AddBattleParticipantAsync(BattleParticipant participant);
        /// <summary>
        /// Gets all battle participants for the given battle ids
        /// </summary>
        Task<List<BattleParticipant>> GetParticipantsByBattleIds(List<Guid> battleIds);
    }
}
