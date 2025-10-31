using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using SyntaxCore.Infrastructure.ErrorExceptions;
using static SyntaxCore.Infrastructure.Middlewares.SignalRExceptionFilter;

namespace SyntaxCore.Infrastructure.Middlewares
{
    public class SignalRExceptionFilter : IHubFilter
    {
        public async ValueTask<object?> InvokeMethodAsync(
            HubInvocationContext context,
            Func<HubInvocationContext, ValueTask<object?>> next,
            ILogger<SignalRExceptionFilter> logger)
        {
            try
            {
                return await next(context);
            }
            catch (Exception ex) when (ex is JoinBattleException or NotFoundException or ForbiddenException)
            {
                logger.LogWarning(ex, "SignalR error in {Method}", context.HubMethodName);

                string message = ex switch
                {
                    JoinBattleException => ex.Message,
                    _ => "An unexpected error occurred. Please try again later."
                };

                await context.Hub.Clients.Caller.SendAsync("HubError", message);
                throw;
            }
        }
    }

}
