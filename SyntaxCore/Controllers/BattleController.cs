using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SyntaxCore.Application.GameSession.Commands.CreateBattle;
using SyntaxCore.Application.GameSession.Commands.CreateNewQuestions;
using SyntaxCore.Application.GameSession.Commands.JoinGameSession;
using SyntaxCore.Infrastructure.SignalRHub;
using SyntaxCore.Models.BattleRelated;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SyntaxCore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("hubs/[controller]")]
    [ApiController]
    public class BattleController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateBattle([FromBody] BattleCreationDto battleCreationDto)
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)!.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var battle = new CreateBattleRequest(
                Guid.Parse(userIdClaim!),
                battleCreationDto.BattleName,
                battleCreationDto.Configurations,
                battleCreationDto.MaxPlayers
            );

            var result = await mediator.Send(battle);

            return Ok(result);
        }
        [HttpPost]
        [Route("create-question-for-battle")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateNewQuestions([FromBody] NewQuestionForBattleDto newQuestionForBattleDto)
        {
            var command = new CreateNewQuestionForBattleRequest(
                newQuestionForBattleDto.QuestionText,
                newQuestionForBattleDto.Category,
                newQuestionForBattleDto.CorrectAnswers,
                newQuestionForBattleDto.WrongAnswers,
                newQuestionForBattleDto.Difficulty,
                newQuestionForBattleDto.TimeForAnswerInSeconds,
                newQuestionForBattleDto.Explanation
            );
            await mediator.Send(command);
            return Ok(new { Message = "New question for battle created successfully." });
        }
    }
}
