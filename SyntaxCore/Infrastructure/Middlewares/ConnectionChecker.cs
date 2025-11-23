using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using SyntaxCore.Infrastructure.DbContext;
using System.Net;
using System.Runtime.CompilerServices;

namespace SyntaxCore.Infrastructure.Middlewares
{
    public static class ConnectionChecker
    {
        public static async Task CheckDatabaseConnection(MyDbContext dbContext, ILogger logger)
        {
            bool supportsUnicode = Console.OutputEncoding.Equals(System.Text.Encoding.UTF8);

            try
            {
                if (await dbContext.Database.CanConnectAsync())
                {
                    var successMsg = supportsUnicode
                        ? "🟢 [DATABASE] Connected successfully."
                        : "[DATABASE OK] Connected successfully.";

                    logger.LogInformation(successMsg);
                }
                else
                {
                    var failMsg = supportsUnicode
                        ? "🔴 [DATABASE] Connection failed."
                        : "[DATABASE FAIL] Connection failed.";

                    logger.LogError(failMsg);
                }
            }
            catch (Exception ex)
            {
                var errorMsg = supportsUnicode
                    ? $"⚠️ [DATABASE] Error: {ex.Message}"
                    : $"[DATABASE ERROR] Error: {ex.Message}";

                logger.LogError(errorMsg);
                throw new Exception("Database connection failed.", ex);
            }
        }

        public static async Task CheckRedisConnectionAsync(IConnectionMultiplexer redis, ILogger logger)
        {
            bool supportsUnicode = Console.OutputEncoding.Equals(System.Text.Encoding.UTF8);

            try
            {
                var db = redis.GetDatabase();
                var pong = await db.PingAsync();

                var successMsg = supportsUnicode
                    ? $"🟢 [REDIS] Connected successfully — Ping: {pong.TotalMilliseconds:F2} ms"
                    : $"[REDIS OK] Connected successfully — Ping: {pong.TotalMilliseconds:F2} ms";

                logger.LogInformation(successMsg);
            }
            catch (RedisConnectionException ex)
            {
                var failMsg = supportsUnicode
                    ? $"🔴 [REDIS] Connection failed — {ex.Message}"
                    : $"[REDIS FAIL] Connection failed — {ex.Message}";

                logger.LogError(failMsg);
                throw new Exception("Redis connection failed.", ex);
            }
            catch (Exception ex)
            {
                var errorMsg = supportsUnicode
                    ? $"⚠️ [REDIS] Unexpected error — {ex.Message}"
                    : $"[REDIS ERROR] Unexpected error — {ex.Message}";

                logger.LogError(errorMsg);
                throw;
            }
        }
        public static async Task CheckAllConnections(MyDbContext dbContext, IConnectionMultiplexer redis, ILogger logger)
        {
             await Task.WhenAll(
                CheckDatabaseConnection(dbContext, logger),
                CheckRedisConnectionAsync(redis, logger)
            );
        }
    }
}
