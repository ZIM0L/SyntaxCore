using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Entities.UserRelated;

[Table("Achievements")]
[Comment("Defines the achievements available in the system.")]
public class Achievement
{
    [Key]
    public Guid AchievementId { get; set; } = Guid.NewGuid();

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public int XpReward { get; set; }

    [MaxLength(255)]
    public string Icon { get; set; } = string.Empty;

    public ICollection<UserAchievement> UserAchievements { get; set; } = null!;
}