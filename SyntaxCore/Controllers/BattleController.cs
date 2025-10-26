using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SyntaxCore.Application.GameSession.Commands.CreateBattle;
using SyntaxCore.Infrastructure.SignalRHub;
using SyntaxCore.Models.BattleRelated;
using System.Threading.Tasks;

namespace SyntaxCore.Controllers
{
    [Route("hubs/[controller]")]
    [ApiController]
    public class BattleController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateBattle([FromBody] BattleCreationDto battleCreationDto)
        {
            var battle = new CreateBattleRequest(battleCreationDto.BattleName, battleCreationDto.QuestionCount, battleCreationDto.TimePerQuestion);
            var result = await mediator.Send(battle);

            return Ok();
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
