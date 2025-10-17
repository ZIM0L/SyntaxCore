using SyntaxCore.Entities.UserRelated;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxCore.Entities;

[Table("BlogPosts")]
public class BlogPost
{
    [Key]
    public int BlogPostId { get; set; }

    [ForeignKey("AuthorFK")]
    public int AuthorId { get; set; }

    [MaxLength(255)]
    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsApproved { get; set; }

    public User Author { get; set; }
    public ICollection<Comment> Comments { get; set; }
}