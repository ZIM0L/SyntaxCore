using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using SyntaxCore.Entities.UserRelated;
using SyntaxCore.Infrastructure.DbContext;

namespace SyntaxCore.Repositories.UserRepository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(MyDbContext context) : base(context) { }
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

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByRefreshToken(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync( u => u.RefreshToken!.Equals(refreshToken));
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<List<User>?> GetUsersByIds(List<Guid> userIds)
        {
            return await _context.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
        }
    }
}
