using MediatR;
using SyntaxCore.Models.UserRelated;

namespace SyntaxCore.Application.Authentication.Commands.RefreshToken
{
    public record RefreshTokenRequest(string RefreshToken) : IRequest<TokenResponseDto>;
}
