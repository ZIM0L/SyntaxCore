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
    }
}
