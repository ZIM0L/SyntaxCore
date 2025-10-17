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
        /// Gets a user by their username and email.
        /// </summary>
        Task<User?> GetUserByEmail(string email);

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        Task<User> AddUser(User user);
    }
}
