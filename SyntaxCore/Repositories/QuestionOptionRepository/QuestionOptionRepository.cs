using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.DbContext;

namespace SyntaxCore.Repositories.QuestionOptionRepository
{
    public class QuestionOptionRepository : BaseRepository, IQuestionOptionRepository
    {
        public QuestionOptionRepository(MyDbContext context) : base(context) { }

        public async Task createQuestionOption(List<QuestionOption> questionOption)
        {
            await _context.QuestionOptions.AddRangeAsync(questionOption);
            await _context.SaveChangesAsync();
        }
    }
}
