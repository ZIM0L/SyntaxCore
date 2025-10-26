using MediatR;
using Microsoft.AspNetCore.SignalR;
using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Entities.UserRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Infrastructure.SignalRHub;
using SyntaxCore.Models.BattleRelated;
using SyntaxCore.Repositories.BattleRepository;
using SyntaxCore.Repositories.UserRepository;

namespace SyntaxCore.Application.GameSession.Commands.CreateBattle
{
    public class CreateBattleHandler(IBattleRepository battleRepository, IUserRepository userRepository,  IHubContext<BattleHub> hubContext) : IRequestHandler<CreateBattleRequest, BattleDto>
    {
        public async Task<BattleDto> Handle(CreateBattleRequest request, CancellationToken cancellationToken)
        {
            var battleDto = new BattleDto();

            if(await userRepository.GetUserById(request.UserId) is not User initPlayer)
            {
                throw new NotFoundException("User not found");
            }
            
            var battle = new Battle(
                battleName: request.BattleName,
                questionsCount: request.QuestionCount
            );

            battle.StartedAt = DateTime.UtcNow;
            battle.Status = BattleStatuses.Waiting;
            battle.Category = request.Category ?? BattleCategory.Bash;

            battle = await battleRepository.CreateBattle(battle) ?? throw new ArgumentException("Battle could not be created");

            battleDto = new BattleDto
            {
                BattleId = battle.BattleId,
                PlayerId1 = initPlayer.Username,
                BattleName = battle.BattleName,
                Category = battle.Category,
                CreatedAt = battle.CreatedAt,
                Status = battle.Status,
                QuestionsCount = battle.QuestionsCount
            };

            await hubContext.Clients.Group($"battle-{battle.BattleName}").SendAsync("BattleCreated", battleDto);

            return battleDto;
        }
    }
}
