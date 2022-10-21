using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domain.Result;
using Backend.Domain.Result.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Result
{
    [ApiController]
    [Route("[controller]")]
    public class LeaderboardController : Controller
    {
        private readonly IMediator _mediator;

        public LeaderboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Single")]
        public async Task<IActionResult> GetLeaderboard()
        {
            //HttpContext.Log();
            var response = await _mediator.Send(new GetLeaderboard.Request());

            if (!response.Success)
            {
                return NotFound(new RouteResponse<List<ResultDTO>>(new(), response.Errors));
            }

            return Ok(new RouteResponse<List<ResultDTO>>(response.Result, response.Errors));
        }

        [HttpGet("Team")]
        public async Task<IActionResult> GetTeamLeaderboard()
        {
            //HttpContext.Log();
            var response = await _mediator.Send(new GetTeamLeaderboard.Request());

            if (!response.Success)
            {
                return NotFound(new RouteResponse<List<TeamResultDTO>>(new(), response.Errors));
            }

            return Ok(new RouteResponse<List<TeamResultDTO>>(response.Result, response.Errors));
        }
    }
}