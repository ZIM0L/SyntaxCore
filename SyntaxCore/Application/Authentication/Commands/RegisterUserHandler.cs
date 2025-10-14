
using MediatR;
using Microsoft.AspNetCore.Identity;
using SyntaxCore.Entities;
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
        var _user = new User();

        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(_user, request.Password);

        
        _user.Username = request.Username;
        _user.PasswordHash = hashedPassword;
        _user.Email = request.Email;

        var doesUserExistsAlready = await _userRepository.IsUserExists(_user);
        if (doesUserExistsAlready)
        {
            return string.Empty;
        }

        var result = await _userRepository.AddUser(_user);
        result
        return 
    }
}
