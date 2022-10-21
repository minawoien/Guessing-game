using System.Threading.Tasks;
using Backend.Domain.Auth.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class LogoutController : Controller
    {
        private readonly IMediator _mediator;

        public LogoutController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task GetAsync()
        {
            //HttpContext.Log();
            await _mediator.Send(new LogoutUser.Request());
        }
    }
}