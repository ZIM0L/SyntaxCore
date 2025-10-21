using MediatR;
using SyntaxCore.Models;

namespace SyntaxCore.Application.Authentication.Commands.RefreshToken
{
    public record RefreshTokenRequest(string RefreshToken) : IRequest<TokenResponseDto>;
}
