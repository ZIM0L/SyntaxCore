using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SyntaxCore.Entities.BattleRelated;

namespace SyntaxCore.Entities.UserRelated;

[Table("Users")]
[Comment("Database for all users, their experience levels, and roles (e.g., player / admin).")]
public class User   
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();
    [Required] 
    public string Email { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    public string Username { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;
    [MaxLength(50)]
    public string? Role { get; set; } = string.Empty;
    [Precision(0)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

    public int Level { get; set; } = 1;            
    public int Experience { get; set; } = 0;       
    public int Wins { get; set; } = 0;            
    public int Losses { get; set; } = 0;          
    public bool IsPublicProfile { get; set; } = true;

    // Navigation
    public ICollection<UserLogin> UserLogins { get; set; }
    public ICollection<UserXpLog> XPLogs { get; set; }
    public ICollection<Battle> currentBattleId { get; set; }
    public ICollection<Battle> BattlesWon { get; set; }
    public ICollection<Achievement> UserAchievements { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<BlogPost> BlogPosts { get; set; }
    public ICollection<AnswerToQuestions> Answers { get; set; }
    public ICollection<QuestionFlag> QuestionFlags { get; set; }
    public ICollection<QuestionFlag>? ReviewedFlags { get; set; }

}