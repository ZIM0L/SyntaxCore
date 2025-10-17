using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxCore.Entities.BattleRelated;

[Table("Questions")]
public class Question
{
    [Key]
    public int QuestionId { get; set; }

    [MaxLength(255)]
    public string Category { get; set; }

    public int Difficulty { get; set; }

    public string QuestionText { get; set; }

    public string CorrectAnswer { get; set; }

    public string Explanation { get; set; }

    public ICollection<AnswerToQuestions> Answers { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<QuestionFlag> Flags { get; set; }
}