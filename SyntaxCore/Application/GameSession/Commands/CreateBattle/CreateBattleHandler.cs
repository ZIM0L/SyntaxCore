using MediatR;
using Microsoft.AspNetCore.SignalR;
using SyntaxCore.Application.GameSession.Commands.JoinGameSession;
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
    public class CreateBattleHandler(IBattleRepository battleRepository, IUserRepository userRepository,  IHubContext<BattleHub> hubContext, IMediator mediator) : IRequestHandler<CreateBattleRequest, BattleDto>
    {
        public async Task<BattleDto> Handle(CreateBattleRequest request, CancellationToken cancellationToken)
        {
            var battleDto = new BattleDto();
            if (await userRepository.GetUserById(request.UserId) is not User initPlayer)
            {
                throw new NotFoundException("User not found");
            }
            
            var battle = new Battle(
                battleName: request.BattleName,
                questionsCount: request.QuestionCount
            );

            battle.StartedAt = DateTime.UtcNow;
            battle.Category = request.Category ?? BattleCategory.Bash;

            battle = await battleRepository.CreateBattle(battle) ?? throw new ArgumentException("Battle could not be created");

            var joinBattleRequest = new JoinBattleRequest(
            request.UserId,
            battle.BattlePublicId,
            ContexRole.Player
            );

            await mediator.Send(joinBattleRequest, cancellationToken);

            battleDto = new BattleDto
            {
                BattleId = battle.BattlePublicId,
                PlayerId1 = initPlayer.Username,
                BattleName = battle.BattleName,
                Category = battle.Category,
                CreatedAt = battle.CreatedAt,
                Status = battle.Status,
                QuestionsCount = battle.QuestionsCount
            };

            await hubContext.Clients.All.SendAsync("BattleCreated", $" Battle created: battle-{battle.BattleName}", battleDto);
            await hubContext.Clients.Group(battleDto.BattleId.ToString()).SendAsync("UserJoinedBattle", $"User {initPlayer.Username} has joined battle {battle.BattleName}");

            return battleDto;
        }
    }
}
