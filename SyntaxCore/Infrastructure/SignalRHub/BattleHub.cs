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

            var playersDictionaryScore = players.ToDictionary(players => players, _ => 0);
            var playersDictionaryAnswers = players.ToDictionary(players => players, _ => false);


            await redisService.SetAsync($"Battle:{battlePublicId}:CurrentIndex", JsonSerializer.SerializeToUtf8Bytes(0), options); // current question index
            await redisService.SetAsync($"Battle:{battlePublicId}:Scores", JsonSerializer.SerializeToUtf8Bytes(playersDictionaryScore), options); // scores
            await redisService.SetAsync($"Battle:{battlePublicId}:Answers", JsonSerializer.SerializeToUtf8Bytes(playersDictionaryAnswers), options); // player answers for current question

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

            // reset answers for the new question
            //await redisService.SetAsync(answersKey, JsonSerializer.SerializeToUtf8Bytes(new Dictionary<Guid, bool>()));

            await Clients.Group(battlePublicId.ToString()).SendAsync("ReceiveQuestion", new 
            {
                QuestionText = allQuestions[currentIndex].QuestionText,
                AllAnswers = allQuestions[currentIndex].AllAnswers.Keys,
                TimeForAnswerInSeconds = allQuestions[currentIndex].TimeForAnswerInSeconds
            });
        }
        //public async Task SubmitAnswer(Guid battlePublicId, List<string> selectedAnswer)
        //{
        //    var userIdClaimId = (Context.User?.Identity as ClaimsIdentity)!.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    var indexKey = $"Battle:{battlePublicId}:CurrentIndex";
        //    var scoresKey = $"Battle:{battlePublicId}:Scores";
        //    var answersKey = $"Battle:{battlePublicId}:Answers";

        //    var currentIndex = JsonSerializer.Deserialize<int>(await redisService.GetAsync(indexKey));
        //    var allQuestions = JsonSerializer.Deserialize<List<QuestionForBattleDto>>(await redisService.GetAsync($"Battle:{battlePublicId.ToString()}"));
        //    var question = allQuestions![currentIndex];

        //    // Sprawdź poprawność
        //    selectedAnswer.ForEach(answer =>
        //    {
        //        if (!question.AllAnswers.TryGetValue(answer, out bool correct) && correct);
        //        {
        //            throw new ArgumentException("Invalid answer submitted");
        //        }
        //    });

        //    var isCorrect = question.AllAnswers.TryGetValue(selectedAnswer, out bool correct) && correct;

        //    // Zapisz odpowiedź gracza
        //    var answers = await redisService.GetAsync<Dictionary<Guid, bool>>(answersKey);
        //    answers[userId] = isCorrect;
        //    await redisService.SetAsync(answersKey, answers);

        //    // Zaktualizuj punkty
        //    var scores = await redisService.GetAsync<Dictionary<Guid, int>>(scoresKey);
        //    if (isCorrect)
        //    {
        //        if (!scores.ContainsKey(userId)) scores[userId] = 0;
        //        scores[userId]++;
        //        await redisService.SetAsync(scoresKey, scores);
        //    }

        //    // Sprawdź czy wszyscy już odpowiedzieli
        //    var connectedUsers = // np. lista graczy z bitwy (pobierz z repo lub redis)
        //    if (answers.Count >= connectedUsers.Count)
        //        {
        //            await redisService.SetAsync(indexKey, currentIndex + 1);
        //            await SendNextQuestion(battlePublicId);
        //        }
        //}

        private async Task SendFinalResults(Guid battlePublicId)
        {
            throw new NotImplementedException();
        }
    }
}
