using System.ComponentModel.DataAnnotations;

namespace SyntaxCore.Entities;

public class User   
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    [Required] 
    public string Email { get; set; } = string.Empty;

    public int Level { get; set; } = 1;            
    public int Experience { get; set; } = 0;       
    public int Wins { get; set; } = 0;            
    public int Losses { get; set; } = 0;          
    public bool IsPublicProfile { get; set; } = true;

}
