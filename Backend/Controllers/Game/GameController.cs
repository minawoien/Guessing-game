using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domain.Auth.Functions;
using Backend.Domain.Game;
using Backend.Domain.Game.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Game
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : Controller
    {
        private readonly IMediator _mediator;

        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            //HttpContext.Log();
            var userId = HttpContext.GetUserId();
            if (userId == 0)
            {
                return Unauthorized(new RouteResponse<GameDTO>(null, new[] {"Not logged in"}));
            }

            var response = await _mediator.Send(new GetGame.Request(userId));
            if (response.Id == 0)
            {
                var err = new List<string>();
                err.Add("Game was not found");
                return NotFound(new RouteResponse<GameDTO>(response, err.ToArray()));
            }

            return Ok(new RouteResponse<GameDTO>(response, null));
        }

        [HttpGet("Quit")]
        public async Task<IActionResult> QuitGame()
        {
            //HttpContext.Log();
            var userId = HttpContext.GetUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            await _mediator.Send(new QuitGame.Request(userId));
            return Ok();
        }
    }
}