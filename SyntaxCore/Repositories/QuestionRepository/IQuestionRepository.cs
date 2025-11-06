using SyntaxCore.Entities.BattleRelated;

namespace SyntaxCore.Repositories.QuestionRepository
{
    public interface IQuestionRepository
    {
        /// <summary>
        /// creates a question in the database
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public Task createQuestion(Question question);

        /// <summary>
        /// gets a random question by category and difficulty
        /// </summary>
        /// <param name="category"></param>
        /// <param name="difficulty"></param>
        /// <param name="questionCountToGet"></param>
        /// <returns></returns>
        Task<List<Question>?> GetRandomQuestionByCategoryAndDifficulty(string category, int difficulty, int questionCountToGet);
    }
}
