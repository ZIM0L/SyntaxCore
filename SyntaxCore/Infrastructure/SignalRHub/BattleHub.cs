using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SyntaxCore.Infrastructure.SignalRHub
{
    [Authorize]
    public class BattleHub : Hub
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
            // Wysyła wiadomość do wszystkich klientów
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
