using SyntaxCore.Entities.UserRelated;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxCore.Entities.BattleRelated;

[Table("Answers")]
public class AnswerToQuestions
{
    [Key]
    public int AnswerId { get; set; }

    [ForeignKey("BattleFK")]
    public int BattleId { get; set; }

    [ForeignKey("QuestionFK")]
    public int QuestionId { get; set; }

    [ForeignKey("UserFK")]
    public int UserId { get; set; }

    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }
    public DateTime AnswerAt { get; set; }

    public Battle Battle { get; set; }
    public Question Question { get; set; }
    public User User { get; set; }
}