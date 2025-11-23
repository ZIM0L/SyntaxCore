using SyntaxCore.Entities.BattleRelated;

namespace SyntaxCore.Repositories.QuestionOptionRepository
{
    public interface IQuestionOptionRepository
    {
        /// <summary>
        /// creates a question option in the database
        /// </summary>
        /// <param name="questionOption"/>
        public Task createQuestionOption(List<QuestionOption> questionOption);
        /// <summary>
        /// gets question options by question id
        /// <paramref name="questionId"/>
        /// <summary></summary>
        public Task<List<QuestionOption>?> GetQuestionOptionsByQuestionId(Guid questionId);
    }
}
