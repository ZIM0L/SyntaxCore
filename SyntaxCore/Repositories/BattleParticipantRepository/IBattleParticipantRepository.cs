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
        public Task<List<BattleParticipant>> GetParticipantsByBattleIds(List<Guid> battleIds);
        /// <summary>
        /// Gets all battle participants count for the given battle id
        /// <paramref name="battleid"/>
        /// <returns>Returns list of participant of given battle id</returns>
        /// </summary>
        public Task<List<BattleParticipant>?> GetParticipantsByBattleId(Guid battleid);
        /// <summary>
        /// Deletes battle participants by battle id.
        /// </summary>
        /// <param name="battleId"></param>
        public Task DeleteBattleParticipants(Guid battleId);

    }
}
