using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SyntaxCore.Application.GameSession.Queries;

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
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task PreviewAllAvailableBattles(GetAllAvailableBattlesRequest getAllAvailableBattles)
        {
            var request = new GetAllAvailableBattlesRequest
            {
                Categories = getAllAvailableBattles.Categories,
                MinQuestionsCount = getAllAvailableBattles.MinQuestionsCount,
                MaxQuestionsCount = getAllAvailableBattles.MaxQuestionsCount
            };
            var allAvaiableBattlesToJoin = await mediator.Send(request);
            await Clients.Caller.SendAsync("ReceiveAllAvailableBattles", allAvaiableBattlesToJoin);
        }
    }
}
