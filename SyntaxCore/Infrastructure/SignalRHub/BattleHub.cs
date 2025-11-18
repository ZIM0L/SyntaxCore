using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using SyntaxCore.Application.GameSession.Commands.JoinGameSession;
using SyntaxCore.Application.GameSession.Queries;
using SyntaxCore.Application.GameSession.Queries.FetchPlayersParticipantsGuids;
using SyntaxCore.Application.GameSession.Queries.FetchQuestionsForBattle;
using SyntaxCore.Application.GameSession.Queries.LeaveBattle;
using SyntaxCore.Constants;
using SyntaxCore.Entities.BattleRelated;
using SyntaxCore.Infrastructure.ErrorExceptions;
using SyntaxCore.Interfaces;
using SyntaxCore.Models.BattleRelated;
using System.Security.Claims;
using System.Text.Json;

namespace SyntaxCore.Infrastructure.SignalRHub
{
    [Authorize]
    public class BattleHub(
        IMediator mediator,
        IDistributedCache redisService
        ) : Hub
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
            var userIdClaimId = (Context.User?.Identity as ClaimsIdentity)!.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userIdClaimName = (Context.User?.Identity as ClaimsIdentity)!.FindFirst(ClaimTypes.Name)?.Value;
            var request = new JoinBattleRequest(
               Guid.Parse(userIdClaimId!),
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
        private async Task StartBattle(Guid battlePublicId)
        {
            Console.WriteLine($"Starting battle {battlePublicId}");
            await Clients.Group(battlePublicId.ToString()).SendAsync("BattleStarting", "Prepare for battle...");

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
            };

            var questions = await mediator.Send(new FetchQuestionsForBattleRequest(battlePublicId));
            var players = await mediator.Send(new FetchPlayersParticipantsGuidsRequest(battlePublicId));

            var playersDictionaryScore = players.Select(playerId => new PlayerRedisData
            {
                playerId = playerId,
                score = 0,
                isSubmitted = false
            }).ToList();

            await redisService.SetAsync($"Battle:{battlePublicId}:CurrentIndex", JsonSerializer.SerializeToUtf8Bytes(0), options); // current question index
            await redisService.SetAsync($"Battle:{battlePublicId}:Scores", JsonSerializer.SerializeToUtf8Bytes(playersDictionaryScore), options); // scores

            await SendNextQuestion(battlePublicId);
        }

        private async Task SendNextQuestion(Guid battlePublicId)
        {
            var indexKey = $"Battle:{battlePublicId}:CurrentIndex"; // current question index
            var answersKey = $"Battle:{battlePublicId}:Answers";

            var currentIndex = JsonSerializer.Deserialize<int>(await redisService.GetAsync(indexKey));
            var allQuestions = JsonSerializer.Deserialize<List<QuestionForBattleDto>>(await redisService.GetAsync($"Battle:{battlePublicId.ToString()}"));

            if (currentIndex >= allQuestions!.Count)
            {
                await SendFinalResults(battlePublicId);
                return;
            }

            var currentQuestion = allQuestions[currentIndex];

            await Clients.Group(battlePublicId.ToString()).SendAsync("ReceiveQuestion", new 
            {
                QuestionText = allQuestions[currentIndex].QuestionText,
                AllAnswers = allQuestions[currentIndex].AllAnswers.Keys.ToList(),
                TimeForAnswerInSeconds = allQuestions[currentIndex].TimeForAnswerInSeconds
            });
        }
        public async Task SubmitAnswer(Guid battlePublicId, List<string> selectedAnswer)
        {
            selectedAnswer.ForEach(x =>
            {
                Console.WriteLine(x);
            });
            var userIdClaimId = (Context.User?.Identity as ClaimsIdentity)!
                .FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new ForbiddenException("Forbbiden action");

            var userId = Guid.Parse(userIdClaimId);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
            };

            var indexKey = $"Battle:{battlePublicId}:CurrentIndex";
            var scoresKey = $"Battle:{battlePublicId}:Scores";

            int currentIndex = JsonSerializer.Deserialize<int>(await redisService.GetAsync(indexKey));

            var allQuestions = JsonSerializer.Deserialize<List<QuestionForBattleDto>>(
                await redisService.GetAsync($"Battle:{battlePublicId}")
            ) ?? throw new BattleException("Internal server error");

            var question = allQuestions[currentIndex];


            foreach (var answer in selectedAnswer)
            {
                if (!question.AllAnswers.ContainsKey(answer))
                    throw new BattleException("Invalid answer submitted");
            }

            bool isCorrect =
                question.AllAnswers.Where(x => x.Value).Select(x => x.Key).OrderBy(x => x)
                .SequenceEqual(selectedAnswer.OrderBy(x => x));

            var scoreRaw = await redisService.GetAsync(scoresKey);
            var scores = JsonSerializer.Deserialize<List<PlayerRedisData>>(scoreRaw) ?? throw new BattleException("Internal server error");

            if(scores.SingleOrDefault(p => p.playerId == userId) is null)
                throw new BattleException("Internal server error");

            scores.SingleOrDefault(p => p.playerId == userId)!.isSubmitted = true;

            if (isCorrect)
                scores.SingleOrDefault(p => p.playerId == userId)!.score++;

            await redisService.SetAsync(
                scoresKey,
                JsonSerializer.SerializeToUtf8Bytes(scores),
                options
            );


            if (!scores.Any(x => x.isSubmitted == false))
            {
                await redisService.SetAsync(
                    indexKey,
                    JsonSerializer.SerializeToUtf8Bytes(currentIndex + 1),
                    options
                );

                scores.ForEach(p => p.isSubmitted = false);

                await redisService.SetAsync(
                    scoresKey,
                    JsonSerializer.SerializeToUtf8Bytes(scores),
                    options
                );

                await SendNextQuestion(battlePublicId);
            }
        }


        private async Task SendFinalResults(Guid battlePublicId)
        {
            var scoresKey = $"Battle:{battlePublicId}:Scores";
            var indexKey = $"Battle:{battlePublicId}:CurrentIndex";

            var scoresBytes = await redisService.GetAsync(scoresKey);
            if (scoresBytes == null) throw new BattleException("Scores not found");

            var scores = JsonSerializer.Deserialize<List<PlayerRedisData>>(scoresBytes)
                ?? throw new BattleException("Scores deserialization failed");

            var finalResults = scores
                .OrderByDescending(p => p.score)
                .Select(p => new
                {
                    PlayerId = p.playerId,
                    Score = p.score
                })
                .ToList();

            await Clients.Group(battlePublicId.ToString()).SendAsync("BattleEnded", new
            {
                BattleId = battlePublicId,
                Results = finalResults
            });

            await redisService.RemoveAsync(scoresKey);
            await redisService.RemoveAsync(indexKey);
            await redisService.RemoveAsync($"Battle:{battlePublicId}:Answers");
            await redisService.RemoveAsync($"Battle:{battlePublicId}");
        }

    }

    internal class PlayerRedisData
    {
        public Guid playerId { get; set; }
        public int score { get; set; }
        public bool isSubmitted { get; set; }
    }
}
