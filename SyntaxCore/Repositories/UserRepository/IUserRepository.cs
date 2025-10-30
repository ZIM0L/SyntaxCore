using SyntaxCore.Entities.UserRelated;

namespace SyntaxCore.Repositories.UserRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Checks if a user with the given username exists in the database.
        /// </summary>
        public Task<bool> IsUserExists(User user);

        /// <summary>
        /// Gets a user by their email.
        /// </summary>
        public Task<User?> GetUserByEmail(string email);

        /// <summary>
        /// Gets a user by their id.
        /// </summary>
        public Task<User?> GetUserById(Guid userId);

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        public Task<User> AddUser(User user);

        /// <summary>
        /// Updates user 
        /// </summary>
        /// <param name="user"></param>
        public Task UpdateUser(User user);

        /// <summary>
        /// Gets user by his refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public Task<User?> GetUserByRefreshToken(string refreshToken);

        /// <summary>
        /// Gets users by their ids.
        /// <summary>
        public Task<List<User>?> GetUsersByIds(List<Guid> userIds);
    }
}
