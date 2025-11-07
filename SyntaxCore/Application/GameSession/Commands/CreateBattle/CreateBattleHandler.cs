using MediatR;
using Microsoft.AspNetCore.SignalR;
using SyntaxCore.Application.GameSession.Commands.JoinGameSession;
using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Entities.UserRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Infrastructure.SignalRHub;
using SyntaxCore.Models.BattleRelated;
using SyntaxCore.Repositories.BattleConfigurationRepository;
using SyntaxCore.Repositories.BattleRepository;
using SyntaxCore.Repositories.UserRepository;
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SyntaxCore.Application.GameSession.Commands.CreateBattle
{
    public class CreateBattleHandler(IBattleRepository battleRepository,
        IUserRepository userRepository,
        IBattleConfigurationRepository battleConfigurationRepository,
        IHubContext<BattleHub> hubContext, 
        IMediator mediator) : IRequestHandler<CreateBattleRequest, BattleDto>
    {
        public async Task<BattleDto> Handle(CreateBattleRequest request, CancellationToken cancellationToken)
        {
            var battleDto = new BattleDto();
            if (await userRepository.GetUserById(request.UserId) is not User initPlayer)
            {
                throw new NotFoundException("User not found");
            }
            
            var battle = new Battle(request.BattleName);

            battle.BattleOwnerFK = initPlayer.UserId;
            battle.Status = BattleStatuses.Waiting;
            battle.maxPlayers = request.MaxPlayers;

            battle = await battleRepository.CreateBattle(battle) ?? throw new ArgumentException("Battle could not be created");

            var battleConfigs = request.Configurations
                 .Select(config => new BattleConfiguration
                 {
                     BattleFK = battle.BattleId,
                     Category = config.Category,
                     Difficulty = config.Difficulty,
                     QuestionCount = config.QuestionCount,
                 })
                 .ToList();
            await battleConfigurationRepository.CreateBattleConfigurationAsync(battleConfigs);

            var joinBattleRequest = new JoinBattleRequest(
            request.UserId,
            battle.BattlePublicId,
            ContexRole.Player
            );

            await mediator.Send(joinBattleRequest, cancellationToken);

            battleDto = new BattleDto
            {
                BattlePublicId = battle.BattlePublicId,
                BattleName = battle.BattleName,
                BattleOwner = initPlayer.Username,
                MaxPlayers = battle.maxPlayers,
                CreatedAt = battle.CreatedAt,
                Status = battle.Status,
                CurrentPlayers = 1,
                TotalQuestionsCount = battleConfigs.Sum(c => c.QuestionCount),
                BattleConfiguration = battleConfigs.Select(c => 
                new BattleConfigurationDto{
                    Category = c.Category,
                    Difficulty = c.Difficulty,
                    QuestionCount = c.QuestionCount
                }).ToList()
            };
            await hubContext.Clients.All.SendAsync("BattleCreated", $" Battle created: battle-{battle.BattleName}", battleDto);
            await hubContext.Clients.Group(battleDto.BattlePublicId.ToString()).SendAsync("UserJoinedBattle", $"User {initPlayer.Username} has joined battle {battle.BattleName}");

            return battleDto;
        }
    }
}
