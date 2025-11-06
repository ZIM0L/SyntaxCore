using MediatR;
using SyntaxCore.Models.BattleRelated;

namespace SyntaxCore.Application.GameSession.Commands.CreateBattle
{
    public record CreateBattleRequest(
    Guid UserId,
    string BattleName,
    List<BattleConfigurationDto> Configurations,
    int MaxPlayers = 2) : IRequest<BattleDto>;
}
