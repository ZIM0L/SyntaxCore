using SyntaxCore.Entities.UserRelated;

namespace SyntaxCore.Repositories.UserRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Checks if a user with the given username exists in the database.
        /// </summary>
        Task<bool> IsUserExists(User user);

        /// <summary>
        /// Gets a user by their email.
        /// </summary>
        Task<User?> GetUserByEmail(string email);

        /// <summary>
        /// Gets a user by their id.
        /// </summary>
        Task<User?> GetUserById(Guid userId);

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        Task<User> AddUser(User user);

        /// <summary>
        /// Updates user 
        /// </summary>
        /// <param name="user"></param>
        Task UpdateUser(User user);

        /// <summary>
        /// Gets user by his refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<User?> GetUserByRefreshToken(string refreshToken);
    }
}
