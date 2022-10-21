using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domain.Auth.Functions;
using Backend.Domain.Game.Pipelines;
using Backend.Domain.Pregame;
using Backend.Domain.Pregame.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.PreGame
{
    [ApiController]
    [Route("[controller]")]
    public class LobbyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LobbyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Join")]
        public async Task<IActionResult> Post(LobbyData lobbyData)
        {
            //HttpContext.Log();
            var userName = HttpContext.GetUserName();
            var userId = HttpContext.GetUserId();
            if (userId == 0)
            {
                return Unauthorized(
                    new RouteResponse<LobbyStatus>(
                        new LobbyStatus(
                            lobbyData.Id, 0),
                        new[] {"Not logged in"}
                    )
                );
            }

            var response = await _mediator.Send(new GetGame.Request(userId));
            if (response.Id != 0)
            {
                List<string> err = new() {"User in another game"};
                return NotFound(new RouteResponse<LobbyStatus>(new LobbyStatus(-1, -1), err.ToArray()));
            }

            var result = await _mediator.Send(new CreateLobby.Request(userId, userName, lobbyData));
            return Created(nameof(Post), new RouteResponse<LobbyStatus>(result.Data, result.Errors));
        }

        [HttpGet]
        public async Task<ActionResult> GetByIdAsync()
        {
            //HttpContext.Log();
            var userId = HttpContext.GetUserId();
            var response = await _mediator.Send(new GetLobbyById.Request(userId));

            if (!response.Success)
            {
                return NotFound(new RouteResponse<LobbyDTO>(response.Data, response.Errors));
            }

            return Ok(new RouteResponse<LobbyDTO>(response.Data, response.Errors));
        }

        [HttpGet("Type/{type}")]
        public async Task<ActionResult> GetByTypeAsync(int type)
        {
            //HttpContext.Log();
            var response = await _mediator.Send(new GetLobbyByType.Request(type));
            if (!response.Success)
            {
                return NotFound(new RouteResponse<List<LobbyDTO>>(response.Data, response.Errors));
            }

            return Ok(new RouteResponse<LobbyDTO[]>(response.Data.ToArray(), response.Errors));
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

            await _mediator.Send(new QuitLobby.Request(userId));
            return Ok();
        }
    }
}