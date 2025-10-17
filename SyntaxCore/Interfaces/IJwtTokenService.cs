using SyntaxCore.Entities.UserRelated;

namespace SyntaxCore.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken(string token);
    }
}
