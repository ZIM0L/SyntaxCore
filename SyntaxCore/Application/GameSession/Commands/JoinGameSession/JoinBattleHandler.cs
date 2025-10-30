using MediatR;
using Microsoft.AspNetCore.SignalR;
using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Entities.UserRelated;
using SyntaxCore.Infrastructure.SignalRHub;
using SyntaxCore.Repositories.BattleParticipantRepository;
using SyntaxCore.Repositories.BattleRepository;
using System.Data;

namespace SyntaxCore.Application.GameSession.Commands.JoinGameSession
{
    public class JoinBattleHandler(IBattleParticipantRepository battleParticipantRepository, IBattleRepository battleRepository, IHubContext<BattleHub> hubContext) : IRequestHandler<JoinBattleRequest, Unit>
    {
        public async Task<Unit> Handle(JoinBattleRequest request, CancellationToken cancellationToken)
        {
            var battle = await battleRepository.GetBattleByPublicId(request.BattlePublicId) ?? throw new ArgumentException("Could not join battle");

            var battleParticipant = new BattleParticipant{
                UserFK = request.UserId,
                BattleFK = battle.BattleId,
                Role = BattleRole.Player,
                Score = 0
            };
            await battleParticipantRepository.AddBattleParticipantAsync(battleParticipant);

            return Unit.Value;
        }
    }
}
