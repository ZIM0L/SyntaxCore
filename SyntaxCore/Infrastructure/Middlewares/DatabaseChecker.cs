using SyntaxCore.Infrastructure.DbContext;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Infrastructure.Middlewares
{
    public static class DatabaseChecker
    {
        public static async Task CheckDatabaseConnection(MyDbContext dbContext, ILogger logger)
        {
            try
            {
                if (await dbContext.Database.CanConnectAsync())
                {
                    logger.LogInformation("Database connection successful.");
                }
                else
                {
                    logger.LogError("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Database connection failed: {Message}", ex.Message);
                throw new Exception("Database connection failed.", ex);
            }
        }
    }
}
