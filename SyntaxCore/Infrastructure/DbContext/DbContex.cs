using Microsoft.EntityFrameworkCore;
using SyntaxCore.Entities.UserRelated;

namespace SyntaxCore.Infrastructure.DbContext;

public class MyDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> op) : base(op) { }

    public DbSet<User> Users { get; set; }
}
