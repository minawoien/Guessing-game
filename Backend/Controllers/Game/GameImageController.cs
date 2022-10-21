using System;
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
    [Route("/Game")]
    public class GameImageController : Controller
    {
        private readonly IMediator _mediator;

        public GameImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Image/{gameId}")]
        public async Task<IActionResult> GetAsync(int gameId)
        {
            //HttpContext.Log();
            var userId = HttpContext.GetUserId();
            var response = await _mediator.Send(new GetFragments.Request(gameId, userId));
            if (!response.Success)
            {
                var err = new List<string>();
                err.Add("Game does not exist");
                return NotFound(new RouteResponse<FragmentDTO[]>(Array.Empty<FragmentDTO>(), err.ToArray()));
            }

            var fragments = response.Paths;
            if (fragments.Length == 0)
            {
                var err = new List<string>();
                err.Add("Image has no unlocked fragments");
                return NotFound(new RouteResponse<string[]>(Array.Empty<string>(), err.ToArray()));
            }

            return Ok(new RouteResponse<FragmentDTO[]>(fragments, Array.Empty<string>()));
        }
    }
}