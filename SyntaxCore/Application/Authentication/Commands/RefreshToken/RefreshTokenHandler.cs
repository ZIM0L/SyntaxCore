using MediatR;
using SyntaxCore.Interfaces;
using SyntaxCore.Models;
using SyntaxCore.Repositories.UserRepository;

namespace SyntaxCore.Application.Authentication.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, TokenResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;
        public RefreshTokenHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
        }
        public async Task<TokenResponseDto> Handle(RefreshTokenRequest? request, CancellationToken cancellationToken)
        {
            var tokenResponse = new TokenResponseDto();
            int tokenExpires = _configuration.GetValue<int>("JWT:RefreshTokenExpiresDays");
            if (request == null || string.IsNullOrEmpty(request.RefreshToken))
            {
                return tokenResponse; //temp
            }
            var user = await _userRepository.GetUserByRefreshToken(request.RefreshToken);
            if (user == null)
            {
                return tokenResponse; // temp
            }

            if (user.RefreshTokenExpiryDateTime <= DateTime.UtcNow)
            {
                return tokenResponse; // temp
            }

            var newAccessToken = _jwtTokenService.GenerateToken(user);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryDateTime = DateTime.UtcNow.AddDays(tokenExpires);

            await _userRepository.UpdateUser(user);

            tokenResponse.AccessToken = newAccessToken;
            tokenResponse.RefreshToken = user.RefreshToken;

            return tokenResponse;
        }
    }
}
