using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SyntaxCore.Application.GameSession.Commands.CreateBattle;
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
                battleCreationDto.QuestionCount,
                battleCreationDto.TimePerQuestion
                );

            var result = await mediator.Send(battle);

            return Ok(result);
        }
        [HttpPost]
        [Route("join")]
        public IActionResult JoinBattle()
        {
            return Ok();
        }
        [HttpPost]
        [Route("answer")]
        public IEnumerable<string> SendAnswers()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost]
        [Route("finish")]
        public IEnumerable<string> fa()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
