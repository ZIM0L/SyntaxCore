using Microsoft.AspNetCore.Identity;
using SyntaxCore.Entities.UserRelated;

namespace SyntaxCore.Infrastructure.Implementations
{
    public static class PasswordHasher 
    {
        public static string HashPassword(this User user, string passwordToHash)
        {
            var hasher = new PasswordHasher<User>();
            return hasher.HashPassword(user, passwordToHash);
        }

        /// <summary>
        /// compares the hashed password with the plain text password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static PasswordVerificationResult VerifyPassword(this User user, string password)
        {
            return new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password);
        }
    }
}
