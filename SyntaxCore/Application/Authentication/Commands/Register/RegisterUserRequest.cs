using MediatR;
using SyntaxCore.Models;

namespace SyntaxCore.Application.Authentication.Commands.Register
{
    public record RegisterUserRequest(
    string Username,
    string Password,
    string Email
    ) : IRequest<TokenResponseDto>;
}
