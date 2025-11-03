using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Entities.BattleRelated;

[Table("Questions")]
[Comment("Stores questions used in battles, including their category, difficulty, correct answer, and explanation.")]
public class Question
{
    [Key]
    public Guid QuestionId { get; set; } = Guid.NewGuid();

    [MaxLength(255)]
    public string Category { get; set; } = string.Empty;
    public int Difficulty { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string? Explanation { get; set; } = string.Empty;
    public int TimeForAnswerInSeconds { get; set; }

    public ICollection<AnswerToQuestions> Answers { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = null!;
    public ICollection<QuestionFlag> Flags { get; set; } = null!;
    public ICollection<QuestionOption> Options { get; set; } = null!;
}