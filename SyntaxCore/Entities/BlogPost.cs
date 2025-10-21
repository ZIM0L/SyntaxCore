using SyntaxCore.Entities.UserRelated;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Entities;

[Table("BlogPosts")]
[Comment("Blog posts created by users, including title, content, author, creation date, and approval status.")]
public class BlogPost
{
    [Key]
    public Guid BlogPostId { get; set; } = Guid.NewGuid();

    public Guid AuthorFK { get; set; }

    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;
    [Column("PostContent")]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public bool IsApproved { get; set; }

    [ForeignKey(nameof(AuthorFK))]
    public User Author { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}