using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SyntaxCore.Application.GameSession.Commands.JoinGameSession;
using SyntaxCore.Application.GameSession.Queries;
using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Models.BattleRelated;

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
                getAllAvailableBattles.Categories,
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

            } catch(JoinBattleException ex)
            {
                await Clients.Caller.SendAsync("HubError", ex.Message);
            }
           
        }
        private async Task StartBattle(Guid BattleIdToStart)
        {
            for (int i = 1; i <= 3; i++)
            {
                await Clients.Group(BattleIdToStart.ToString()).SendAsync("Countdown", i);
                await Task.Delay(1000);
            }
        }
        public async Task ReceiveQuestion(QuestionForBattleDto questionForBattleDto)
        {

        }
        public async Task SubmitAnswer()
        {

        }
        public async Task BattleFinished()
        {

        }
        public async Task LeaveBattle()
        {

        }
    }
}
