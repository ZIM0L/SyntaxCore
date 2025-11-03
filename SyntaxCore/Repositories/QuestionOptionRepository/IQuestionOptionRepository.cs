using SyntaxCore.Entities.BattleRelated;

namespace SyntaxCore.Repositories.QuestionOptionRepository
{
    public interface IQuestionOptionRepository
    {
        /// <summary>
        /// creates a question option in the database
        /// </summary>
        /// <param name="questionOption"></param>
        /// <returns></returns>
        public Task createQuestionOption(List<QuestionOption> questionOption);
    }
}
