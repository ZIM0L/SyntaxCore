using MediatR;
using SyntaxCore.Models.BattleRelated;

namespace SyntaxCore.Application.GameSession.Queries
{
    public record GetAllAvailableBattlesRequest( 
        string BattleName,
        List<string>? Categories,
        int DifficultyLevel,
        int? MinQuestionsCount,
        int? MaxQuestionsCount 
    ) : IRequest<List<BattleDto>>;
}
