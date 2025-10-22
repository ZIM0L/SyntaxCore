using MediatR;
using Microsoft.AspNetCore.Identity;
using SyntaxCore.Entities.UserRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Infrastructure.Implementations;
using SyntaxCore.Interfaces;
using SyntaxCore.Models;
using SyntaxCore.Repositories.UserRepository;

namespace SyntaxCore.Application.Authentication.Queries.Login
{

    public class LoginUserHandler(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IConfiguration configuration)
        : IRequestHandler<LoginUserRequest, TokenResponseDto>
    {
        public async Task<TokenResponseDto> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetUserByEmail(request.Email);
            int tokenExpires = configuration.GetValue<int>("JWT:RefreshTokenExpiresDays");
            var tokenResponse = new TokenResponseDto();

            if (user is not null && user.VerifyPassword(request.Password) == PasswordVerificationResult.Success)
            {
                var accessToken =  jwtTokenService.GenerateToken(user);
                var refreshToken = jwtTokenService.GenerateRefreshToken();

                tokenResponse.AccessToken = accessToken;
                tokenResponse.RefreshToken = refreshToken;
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryDateTime = DateTime.UtcNow.AddDays(tokenExpires);
                await userRepository.UpdateUser(user);
                return tokenResponse;
            }
            throw new NotFoundException("Invalid Credentials");
        }
    }
}
