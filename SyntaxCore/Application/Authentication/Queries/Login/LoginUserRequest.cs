using MediatR;
using SyntaxCore.Models;

namespace SyntaxCore.Application.Authentication.Queries.Login
{
    public record LoginUserRequest(string Email, string Password) : IRequest<TokenResponseDto>;
}
