using MediatR;
using Microsoft.AspNetCore.Identity;
using SyntaxCore.Entities.UserRelated;
using SyntaxCore.Infrastructure.Implementations;
using SyntaxCore.Interfaces;
using SyntaxCore.Models;
using SyntaxCore.Repositories.UserRepository;

namespace SyntaxCore.Application.Authentication.Queries.Login
{

    public class LoginUserHandler : IRequestHandler<LoginUserRequest, TokenResponseDto>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public LoginUserHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IConfiguration configuration)
        {
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
            _userRepository = userRepository;
        }
        public async Task<TokenResponseDto> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserByEmail(request.Email);
            int tokenExpires = _configuration.GetValue<int>("JWT:RefreshTokenExpiresDays");
            var tokenResponse = new TokenResponseDto();

            if (user is not null && user.VerifyPassword(request.Password) == PasswordVerificationResult.Success)
            {
                var accessToken =  _jwtTokenService.GenerateToken(user);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                tokenResponse.AccessToken = accessToken;
                tokenResponse.RefreshToken = refreshToken;
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryDateTime = DateTime.UtcNow.AddDays(tokenExpires);
                await _userRepository.UpdateUser(user);

            }
            return tokenResponse;
        }
    }
}
