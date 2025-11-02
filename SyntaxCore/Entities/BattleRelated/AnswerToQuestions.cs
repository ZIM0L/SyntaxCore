using SyntaxCore.Entities.UserRelated;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Entities.BattleRelated;

[Table("Answers")]
[Comment("Stores answers provided by users to questions during battles.")]
public class AnswerToQuestions
{
    [Key]
    public Guid AnswerId { get; set; } = Guid.NewGuid();
    public Guid BattleFK { get; set; }
    public Guid QuestionFK { get; set; }
    public Guid UserFK { get; set; }
    public string SelectedOptionAnswer { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }
    public DateTime AnswerAt { get; set; }

    // Navigation 
    [ForeignKey(nameof(BattleFK))]
    public Battle Battle { get; set; } = null!;

    [ForeignKey(nameof(QuestionFK))]
    public Question Question { get; set; } = null!;

    [ForeignKey(nameof(UserFK))]
    public User User { get; set; } = null!;
}
