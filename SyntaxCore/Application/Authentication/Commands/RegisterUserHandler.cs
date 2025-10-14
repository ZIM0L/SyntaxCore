
using MediatR;
using Microsoft.AspNetCore.Identity;
using SyntaxCore.Entities;
using SyntaxCore.Infrastructure.Implementations;
using SyntaxCore.Interfaces;
using SyntaxCore.Repositories.UserRepository;

namespace SyntaxCore.Application.Authentication.Commands;

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    public RegisterUserHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }
    public async Task<string> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Username = request.Username,
            Email = request.Email
        };
        user.PasswordHash = user.HashPassword(request.Password);


        var doesUserExistsAlready = await _userRepository.IsUserExists(user);
        if (doesUserExistsAlready)
        {
            return string.Empty;
        }

        var resultUser = await _userRepository.AddUser(user);

        return _jwtTokenService.GenerateToken(resultUser);
    }
}
