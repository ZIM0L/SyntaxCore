using MediatR;
using Microsoft.AspNetCore.SignalR;
using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.SignalRHub;
using SyntaxCore.Models.BattleRelated;
using SyntaxCore.Repositories.BattleRepository;

namespace SyntaxCore.Application.GameSession.Commands.CreateBattle
{
    public class CreateBattleHandler(IBattleRepository battleRepository, IHubContext<BattleHub> hubContext) : IRequestHandler<CreateBattleRequest, BattleDto>
    {
        public async Task<BattleDto> Handle(CreateBattleRequest request, CancellationToken cancellationToken)
        {
            var battleDto = new BattleDto();

            var battle = new Battle(
                battleName: request.BattleName,
                questionsCount: request.QuestionCount
            );

            battle.StartedAt = DateTime.UtcNow;
            battle.Status = BattleStatuses.Waiting;
            battle.Category = request.Category ?? BattleCategory.Bash;

            if(await battleRepository.CreateBattle(battle) is not Battle)
            {
                throw new ArgumentException("Battle could not be created");
            }
       
            await hubContext.Clients.All.SendAsync("BattleCreated", new BattleDto[] { battleDto });

            return battleDto;
        }
    }
}
