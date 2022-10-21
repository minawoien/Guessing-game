using System.Threading.Tasks;
using Backend.Domain.Auth.Functions;
using Backend.Domain.Pregame.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.PreGame
{
    [ApiController]
    [Route("/Lobby")]
    public class StartController : Controller
    {
        private readonly IMediator _mediator;

        public StartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Start")]
        public async Task<IActionResult> GetAsync()
        {
            //HttpContext.Log();
            var userId = HttpContext.GetUserId();


            var response = await _mediator.Send(new StartGame.Request(userId));
            if (response.Data == -1)
            {
                return NotFound(new RouteResponse<int>(response.Data, response.Errors));
            }

            return Ok(new RouteResponse<int>(response.Data, response.Errors));
        }
    }
}