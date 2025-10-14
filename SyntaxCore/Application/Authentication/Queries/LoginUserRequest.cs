using MediatR;

namespace SyntaxCore.Application.Authentication.Queries
{
    public record LoginUserRequest(string Email, string Password) : IRequest<string>;
}
