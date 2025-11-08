using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.DbContext;
using SyntaxCore.Models.BattleRelated;

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

        public async Task DeleteBattle(Guid battleId)
        {
            var battle = _context.Battles.FirstOrDefault(b => b.BattleId == battleId);
            _context.Battles.Remove(battle!);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BattleDto>> GetAllAvailableBattlesWithFilters(
            string? battleName,
            List<string>? categories,
            int? difficultyLevel,
            int? minQuestionsCount,
            int? maxQuestionsCount)
        {
            var query = _context.Battles
                .AsNoTracking()
                .Include(b => b.BattleParticipants)
                .Include(b => b.BattleConfigurations)
                .Where(b => b.Status == BattleStatuses.Waiting)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(battleName))
            {
                query = query.Where(b => EF.Functions.Like(b.BattleName, $"%{battleName}%"));
            }

            if (categories is { Count: > 0 })
            {
                query = query.Where(b =>
                    b.BattleConfigurations.Any(cfg => categories.Contains(cfg.Category)));
            }

            if (difficultyLevel > 0)
            {
                query = query.Where(b =>
                    b.BattleConfigurations.Any(cfg => cfg.Difficulty == difficultyLevel));
            }

            var battles = await query
                .Select(b => new BattleDto
                {
                    BattlePublicId = b.BattlePublicId,
                    BattleName = b.BattleName,
                    BattleOwner = b.BattleOwner.Username,
                    CreatedAt = b.CreatedAt,
                    StartedAt = b.StartedAt,
                    EndedAt = b.EndedAt,
                    Status = b.Status,
                    MaxPlayers = b.maxPlayers,
                    CurrentPlayers = b.BattleParticipants.Count(),
                    TotalQuestionsCount = b.BattleConfigurations.Sum(cfg => cfg.QuestionCount),
                    BattleConfiguration = b.BattleConfigurations.Select(cfg => new BattleConfigurationDto
                    {
                        Category = cfg.Category,
                        Difficulty = cfg.Difficulty,
                        QuestionCount = cfg.QuestionCount,
                    }).ToList()
                })
                .ToListAsync();

            return battles;
        }

        public async Task<List<Battle>?> GetAllGamesWithWaitingStatusAsync(string? battleName)
        {
            return await _context.Battles
                   .Where(b => b.Status == BattleStatuses.Waiting)
                   .Where(b => string.IsNullOrEmpty(battleName) || EF.Functions.Like(b.BattleName, $"%{battleName}%"))
                   .ToListAsync();
        }

        public async Task<Battle?> GetBattleByPublicId(Guid battlePublicId)
        {
            return await _context.Battles.FirstOrDefaultAsync(b => b.BattlePublicId == battlePublicId);
        }

        public async Task UpdateBattleStatus(Guid battlePublicId, string? status)
        {
            await _context.Battles
                .Where(b => b.BattlePublicId == battlePublicId)
                .ExecuteUpdateAsync(b => b.SetProperty(b => b.Status, status));
        }
    }
}
