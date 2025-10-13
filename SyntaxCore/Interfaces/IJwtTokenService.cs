using SyntaxCore.Entities;

namespace SyntaxCore.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken(string token);
    }
}
