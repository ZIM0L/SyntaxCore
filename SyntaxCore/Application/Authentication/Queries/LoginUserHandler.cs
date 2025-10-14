using MediatR;
using Microsoft.AspNetCore.Identity;
using SyntaxCore.Entities;
using SyntaxCore.Infrastructure.Implementations;
using SyntaxCore.Interfaces;
using SyntaxCore.Models;
using SyntaxCore.Repositories.UserRepository;

namespace SyntaxCore.Application.Authentication.Queries
{

    public class LoginUserHandler : IRequestHandler<LoginUserRequest, string>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserRepository _userRepository;
        public LoginUserHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
            _userRepository = userRepository;
        }
        public async Task<string> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserByEmail(request.Email);

            if (user is not null && user.VerifyPassword(request.Password) == PasswordVerificationResult.Success)
            {
                return _jwtTokenService.GenerateToken(user);
            }
            return string.Empty;
        }
    }
}
