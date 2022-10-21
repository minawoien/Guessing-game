using System.Threading.Tasks;
using Backend.Domain.Auth.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegisterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(LogRegData registerData)
        {
            //HttpContext.Log();
            var result = await _mediator.Send(new RegisterUser.Request(registerData));
            if (result.Success)
            {
                return Created(nameof(Post), new RouteResponse<string>(registerData.UserName, result.Errors));
            }

            return Ok(new RouteResponse<string>(registerData.UserName, result.Errors));
        }
    }
}