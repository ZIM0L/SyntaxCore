using MediatR;
using Microsoft.AspNetCore.Identity;
using SyntaxCore.Entities;
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
            string token = string.Empty;
            User? user = await _userRepository.GetUserByUsername(request.Email, request.Password);

            if (user != null)
            {
                var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    return _jwtTokenService.GenerateToken(user);
                }
            }
            return String.Empty;
        }
    }
}
