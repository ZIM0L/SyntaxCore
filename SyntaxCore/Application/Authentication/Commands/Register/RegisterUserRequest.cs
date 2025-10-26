using MediatR;
using SyntaxCore.Models.UserRelated;

namespace SyntaxCore.Application.Authentication.Commands.Register
{
    public record RegisterUserRequest(
    string Username,
    string Password,
    string Email
    ) : IRequest<TokenResponseDto>;
}
