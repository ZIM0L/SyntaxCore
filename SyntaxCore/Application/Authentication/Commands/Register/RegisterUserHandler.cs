
using MediatR;
using SyntaxCore.Application.Authentication.Queries.Login;
using SyntaxCore.Entities.UserRelated;
using SyntaxCore.Infrastructure.Implementations;
using SyntaxCore.Interfaces;
using SyntaxCore.Models;
using SyntaxCore.Repositories.UserRepository;

namespace SyntaxCore.Application.Authentication.Commands.Register;

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, TokenResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMediator _mediator;
    public RegisterUserHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IMediator mediator)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _mediator = mediator;
    }
    public async Task<TokenResponseDto> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
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
            throw new InvalidOperationException("User with this email or username already exists."); //temp error
        }
        var resultUser = await _userRepository.AddUser(user);

        var loginRequest = new LoginUserRequest(resultUser.Email,request.Password);
        var loginResult = await _mediator.Send(loginRequest, cancellationToken);

        return loginResult;
    }
}
