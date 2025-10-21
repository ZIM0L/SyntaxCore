using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Entities.UserRelated;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Entities;

[Table("Comments")]
[Comment("Comments made by users on blog posts and questions.")]
public class Comment
{
    [Key]
    public Guid CommentId { get; set; } = Guid.NewGuid();

    public Guid UserFK { get; set; }

    public Guid? BlogPostFK { get; set; }

    public Guid? QuestionFK { get; set; }
    [Column("CommentContent")]
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(UserFK))]
    public User User { get; set; } = null!;
    [ForeignKey(nameof(BlogPostFK))]
    public BlogPost BlogPost { get; set; } = null!;
    [ForeignKey(nameof(QuestionFK))]
    public Question Question { get; set; } = null!;
}
