using MediatR;
using Microsoft.AspNetCore.SignalR;
using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Entities.UserRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Infrastructure.SignalRHub;
using SyntaxCore.Models.BattleRelated;
using SyntaxCore.Repositories.BattleParticipantRepository;
using SyntaxCore.Repositories.BattleRepository;
using SyntaxCore.Repositories.UserRepository;
using System.Data;

namespace SyntaxCore.Application.GameSession.Commands.JoinGameSession
{
    public class JoinBattleHandler(IBattleParticipantRepository battleParticipantRepository, IBattleRepository battleRepository, IUserRepository userRepository) : IRequestHandler<JoinBattleRequest, BattleParticipantsDto>
    {
        public async Task<BattleParticipantsDto> Handle(JoinBattleRequest request, CancellationToken cancellationToken)
        {
            var battle = await battleRepository.GetBattleByPublicId(request.BattlePublicId) ?? throw new JoinBattleException("Could not join battle");
            var participantsBeforeJoin  = await battleParticipantRepository.GetParticipantsByBattleId(battle.BattleId) ?? new List<BattleParticipant>();

            participantsBeforeJoin.ForEach(p =>
            {
                if (p.UserFK == request.UserId)
                {
                    throw new JoinBattleException("User already joined the battle");
                }
            });

            if (participantsBeforeJoin.Count >= battle.maxPlayers)
            {
                throw new JoinBattleException("Battle is full");
            }

            var battleParticipant = new BattleParticipant{
                UserFK = request.UserId,
                BattleFK = battle.BattleId,
                Role = ContexRole.Player,
                Score = 0
            };
            await battleParticipantRepository.AddBattleParticipantAsync(battleParticipant);

            var participantsAfterJoin = await battleParticipantRepository
               .GetParticipantsByBattleId(battle.BattleId) ?? new List<BattleParticipant>();

            var userIds = participantsAfterJoin.Select(p => p.UserFK).ToList();

            if (participantsAfterJoin.Count == battle.maxPlayers)
            {
                await battleRepository.UpdateBattleStatus(battle.BattleId, BattleStatuses.InProgress);
            }

            var users = await userRepository.GetUsersByIds(userIds) ?? new List<User>();

            var playerInitJoining = users.Where( u => u.UserId == request.UserId).Select(u => u.Username).First();

            var PlayersInGameDto = new BattleParticipantsDto
            {
                CurrentJoinedPlayerUserName = playerInitJoining,
                PlayersUserNames = users.Select(u => u.Username).ToList(),
                MaxParticipants = battle.maxPlayers
            };
            return PlayersInGameDto;
        }
    }
}
