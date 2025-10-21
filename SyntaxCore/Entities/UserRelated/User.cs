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
    [Column("UserLevel")]
    public int Level { get; set; } = 1;            
    public int Experience { get; set; } = 0;       
    public int Wins { get; set; } = 0;            
    public int Losses { get; set; } = 0;          
    public bool IsPublicProfile { get; set; } = true;
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime? RefreshTokenExpiryDateTime { get; set; }

    // Navigation
    public ICollection<UserLogin> UserLogins { get; set; } = null!;
    public ICollection<UserXpLog> XPLogs { get; set; } = null!;
    public ICollection<Battle> BattlesWon { get; set; } = null!;
    public ICollection<Achievement> UserAchievements { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = null!;
    public ICollection<BlogPost> BlogPosts { get; set; } = null!;
    public ICollection<AnswerToQuestions> Answers { get; set; } = null!;
    public ICollection<QuestionFlag> QuestionFlags { get; set; } = null!;
    public ICollection<QuestionFlag>? ReviewedFlags { get; set; } = null!;
    public ICollection<BattleParticipant>? BattleParticipants { get; set; } = null!;

}