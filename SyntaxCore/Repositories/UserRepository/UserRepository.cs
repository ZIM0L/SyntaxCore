using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using SyntaxCore.Entities;
using SyntaxCore.Infrastructure.DbContext;

namespace SyntaxCore.Repositories.UserRepository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(MyDbContext context) : base(context)
        {

        }

        public async Task<bool> IsUserExists(User user)
        {
            return await _context.Users.AnyAsync(u => u.Email == user.Email);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
