using Microsoft.EntityFrameworkCore;
using SyntaxCore.Infrastructure.DbContext;

public abstract class BaseRepository
{
    protected readonly MyDbContext _context;
    protected BaseRepository(MyDbContext context)
	{
        _context = context;
	}
}
