using Microsoft.EntityFrameworkCore;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.DbContext;

namespace SyntaxCore.Repositories.QuestionRepository
{
    public class QuestionRepository : BaseRepository, IQuestionRepository
    {
        public QuestionRepository(MyDbContext context) : base(context) { }

        public async Task createQuestion(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Question>?> GetRandomQuestionByCategoryAndDifficulty(string category, int difficulty, int questionCountToGet)
        {
            return await _context.Questions
                .Where(q => q.Category == category && q.Difficulty == difficulty)
                .OrderBy(q => Guid.NewGuid())
                .Take(questionCountToGet)
                .ToListAsync();
        }
    }
}
