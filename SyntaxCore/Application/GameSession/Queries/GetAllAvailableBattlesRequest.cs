using MediatR;
using SyntaxCore.Models.BattleRelated;

namespace SyntaxCore.Application.GameSession.Queries
{
    public record GetAllAvailableBattlesRequest( 
        List<string>? Categories,
        int? MinQuestionsCount,
        int? MaxQuestionsCount 
    ) : IRequest<List<BattleDto>>;
}
