using System.Threading.Tasks;
using Backend.Domain.Auth.Functions;
using Backend.Domain.Game;
using Backend.Domain.Game.Pipelines;
using Backend.Domain.Images.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Game
{
    [ApiController]
    [Route("Game")]
    public class ProposeController : Controller
    {
        private readonly IMediator _mediator;

        public ProposeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Propose")]
        public async Task<IActionResult> Post(ProposeData proposeData)
        {
            //HttpContext.Log();
            var userId = HttpContext.GetUserId();

            //get imageId and game from gameservice for proposer
            var proposerGame = await _mediator
                .Send(new GetProposerGame.Request(userId));
            if (!proposerGame.Success)
            {
                return NotFound(new RouteResponse<string>(null, proposerGame.Errors));
            }

            //get fragment from imagedomain
            var fragment = await _mediator
                .Send(new ResolveLayer.Request(proposerGame.Data.ImageId, proposeData.X, proposeData.Y));

            //unlock fragment with gameservice
            var result = await _mediator.Send(new UnlockWithProposer.Request(proposerGame.Data.GameId, fragment));
            return Ok();
        }
    }
}