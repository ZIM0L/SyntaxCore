using MediatR;
using SyntaxCore.Models.BattleRelated;

namespace SyntaxCore.Application.GameSession.Queries
{
    public record GetAllAvailableBattlesRequest : IRequest<List<BattleDto>>
    {
        public List<string>? Categories { get; init; }
        public int? MinQuestionsCount { get; init; }
        public int? MaxQuestionsCount { get; init; }
    }
}
