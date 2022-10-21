using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domain.Auth.Functions;
using Backend.Domain.Game.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Game
{
    [ApiController]
    [Route("/Game")]
    public class GuessController : Controller
    {
        private readonly IMediator _mediator;

        public GuessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Guess")]
        public async Task<IActionResult> PostAsync(GuessData guess)
        {
            //HttpContext.Log();
            var userId = HttpContext.GetUserId();
            var response = await _mediator.Send(new RegisterGuess.Request(guess.Value, userId));
            if (!response.Success)
            {
                List<string> err = new();
                err.Add("Failed to register guess");
                return NotFound(new RouteResponse<string>(guess.Value, err.ToArray()));
            }

            return Ok(new RouteResponse<string>(guess.Value, Array.Empty<string>()));
        }
    }
}