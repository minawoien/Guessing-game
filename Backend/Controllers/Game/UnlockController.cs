using System;
using System.Threading.Tasks;
using Backend.Domain.Auth.Functions;
using Backend.Domain.Game.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Game
{
    [ApiController]
    [Route("Game")]
    public class UnlockController : Controller
    {
        private readonly IMediator _mediator;

        public UnlockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //this route is temporary, it is only used ot unlock segments for testing.
        [HttpGet("Unlock")]
        public async Task<IActionResult> Get()
        {
            //HttpContext.Log();
            var userId = HttpContext.GetUserId();
            var gameId = await _mediator.Send(new GetGameId.Request(userId));
            if (gameId == 0)
            {
                return BadRequest(new RouteResponse<string>("Failed", Array.Empty<string>()));
            }

            var response = await _mediator.Send(new UnlockWithOracle.Request(gameId));
            if (response.Success)
            {
                return Ok(new RouteResponse<string>("Success", Array.Empty<string>()));
            }
            else
            {
                await _mediator.Send(new SetWaitingStatus.Request(gameId));
                return Ok(new RouteResponse<string>("Success", Array.Empty<string>()));
            }
        }
    }
}