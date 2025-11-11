using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SyntaxCore.Application.GameSession.Commands.JoinGameSession;
using SyntaxCore.Application.GameSession.Queries;
using SyntaxCore.Application.GameSession.Queries.FetchQuestionsForBattle;
using SyntaxCore.Application.GameSession.Queries.LeaveBattle;
using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Models.BattleRelated;
using System.Security.Claims;

namespace SyntaxCore.Infrastructure.SignalRHub
{
    [Authorize]
    public class BattleHub(IMediator mediator) : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("A user connected to BattleHub: " + Context.ConnectionId);


            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine("A user disconnected from BattleHub: " + Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        public async Task PreviewAllAvailableBattles(GetAllAvailableBattlesRequest getAllAvailableBattles)
        {
            var request = new GetAllAvailableBattlesRequest(
                getAllAvailableBattles.BattleName,
                getAllAvailableBattles.Categories,
                getAllAvailableBattles.DifficultyLevel,
                getAllAvailableBattles.MinQuestionsCount,
                getAllAvailableBattles.MaxQuestionsCount
                );
            var allAvaiableBattlesToJoin = await mediator.Send(request);
            await Clients.Caller.SendAsync("ReceiveAllAvailableBattles", allAvaiableBattlesToJoin);
        }

        public async Task JoinBattle(JoinBattleRequest joinBattleRequest)
        {
            var request = new JoinBattleRequest(
               joinBattleRequest.UserId,
               joinBattleRequest.BattlePublicId,
               ContexRole.Player
               );
            try
            {
                var players = await mediator.Send(request);

                await Groups.AddToGroupAsync(Context.ConnectionId, joinBattleRequest.BattlePublicId.ToString());

                await Clients.Groups(joinBattleRequest.BattlePublicId.ToString()).SendAsync("UserJoinedBattle", $"User {players.CurrentJoinedPlayerUserName} has joined the battle. Players count: {players.PlayersUserNames.Count}/{players.MaxParticipants}");

                await Clients.Caller.SendAsync("JoinBattleSuccess", $"Joined battle {joinBattleRequest.BattlePublicId}");

                Console.WriteLine($"Connection {Context.ConnectionId} joined battle group {joinBattleRequest.BattlePublicId.ToString()}");

                if (players.PlayersUserNames.Count == players.MaxParticipants)
                {
                    await Clients.Groups(joinBattleRequest.BattlePublicId.ToString()).SendAsync("BattleIsReady", $"Battle is ready. Prepare ");
                    await Task.Delay(2000);
                    await StartBattle(joinBattleRequest.BattlePublicId);
                }

            }
            catch (JoinBattleException ex)
            {
                await Clients.Caller.SendAsync("HubError", ex.Message);
            }

        }
        private async Task StartBattle(Guid battlePublicIdToStart)
        {
            for (int i = 1; i <= 3; i++)
            {
                await Clients.Group(battlePublicIdToStart.ToString()).SendAsync("Countdown", i);
                await Task.Delay(1000);
            }
            var request = new FetchQuestionsForBattleRequest(battlePublicIdToStart);
            await mediator.Send(request);
        }
        public async Task LeaveBattle(Guid battlePublicId)
        {
            var userIdClaimId = (Context.User?.Identity as ClaimsIdentity)!.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userIdClaimName = (Context.User?.Identity as ClaimsIdentity)!.FindFirst(ClaimTypes.Name)?.Value;
            Console.WriteLine($"User {userIdClaimName} with ID {userIdClaimId} is leaving battle {battlePublicId}");

            var request = new LeaveBattleRequest(
                battlePublicId,
                Guid.Parse(userIdClaimId!)
                );
            try
            {
                await mediator.Send(request);

                await Clients.Group(battlePublicId.ToString()).SendAsync("UserLeftBattle", $"User {userIdClaimName} has left the battle.");
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, battlePublicId.ToString());
            }
            catch (BattleException ex)
            {
                await Clients.Caller.SendAsync("HubError", ex.Message);

            }
        }
    }
}
