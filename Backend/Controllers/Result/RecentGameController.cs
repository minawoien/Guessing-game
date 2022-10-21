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
    public class RecentGamesController : Controller
    {
        private readonly IMediator _mediator;

        public RecentGamesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecentGames()
        {
            //HttpContext.Log();
            var response = await _mediator.Send(new GetRecentGames.Request());

            if (!response.Success)
            {
                return NotFound(new RouteResponse<List<RecentGameDTO>>(new(), response.Errors));
            }

            return Ok(new RouteResponse<List<RecentGameDTO>>(response.Result, response.Errors));
        }
    }
}