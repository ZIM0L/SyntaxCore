using MediatR;
using SyntaxCore.Models.BattleRelated;

namespace SyntaxCore.Application.GameSession.Commands.CreateBattle
{
    public record CreateBattleRequest(
    string BattleName,
    int QuestionCount, 
    int? TimePerQuestion,
    string? Category = null
    ) : IRequest<BattleDto>;
}
