using MediatR;
using SyntaxCore.Models.UserRelated;

namespace SyntaxCore.Application.Authentication.Queries.Login
{
    public record LoginUserRequest(string Email, string Password) : IRequest<TokenResponseDto>;
}
