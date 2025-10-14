using MediatR;
using SyntaxCore.Entities;

namespace SyntaxCore.Application.Authentication.Commands
{
    public record RegisterUserRequest(
    string Username,
    string Password,
    string Email
    ) : IRequest<string>;
}
