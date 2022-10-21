using System.Threading.Tasks;
using Backend.Domain.Auth.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(LogRegData loginData)
        {
            //HttpContext.Log();
            var result = await _mediator.Send(new LoginUser.Request(loginData));
            if (result.Success)
            {
                return Ok(new RouteResponse<string>(loginData.UserName, result.Errors));
            }

            return Unauthorized(new RouteResponse<string>(loginData.UserName, result.Errors));
        }
    }
}