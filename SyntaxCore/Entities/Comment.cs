using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Entities.UserRelated;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxCore.Entities;

[Table("Comments")]
public class Comment
{
    [Key]
    public int CommentId { get; set; }

    [ForeignKey("UserFK")]
    public int UserId { get; set; }

    [ForeignKey("BlogPostFK")]
    public int? PostId { get; set; }

    [ForeignKey("QuestionFK")]
    public int? QuestionId { get; set; }

    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; }
    public BlogPost BlogPost { get; set; }
    public Question Question { get; set; }
}
