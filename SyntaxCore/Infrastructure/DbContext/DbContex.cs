using Microsoft.EntityFrameworkCore;
using SyntaxCore.Entities;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Entities.UserRelated;

namespace SyntaxCore.Infrastructure.DbContext;

public class MyDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> op) : base(op) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Battle>()
            .HasOne(b => b.BattleOwner)
            .WithMany(u => u.BattlesOwn)
            .HasForeignKey(b => b.BattleOwnerFK)
            .OnDelete(DeleteBehavior.Restrict); 
    }

    public DbSet<AnswerToQuestions> AnswersToQuestions { get; set; }
    public DbSet<BattleConfiguration> BattleConfigurations { get; set; }
    public DbSet<Battle> Battles { get; set; }
    public DbSet<QuestionOption> QuestionOptions { get; set; }
    public DbSet<BattleParticipant> BattleParticipants { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionFlag> QuestionFlags { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<UserAchievement> UserAchievements{ get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserLoginHistory> UserLoginHistories { get; set; }
    public DbSet<UserXpLog> UserXpLogs { get; set; }
    public DbSet<BlogPost> BlogPosts{ get; set; }
    public DbSet<Comment> Comments{ get; set; }

}
