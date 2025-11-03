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
    }
}
