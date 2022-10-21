using System.Threading.Tasks;
using Backend.Domain.Images.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Image
{
    [ApiController]
    [Route("[controller]")]
    public class ImageFragmentController : Controller
    {
        private readonly IMediator _mediator;

        public ImageFragmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{image}/{filename}")]
        public async Task<IActionResult> GetAsync(int image, string filename)
        {
            //HttpContext.Log();
            var fragment = await _mediator.Send(new GetImageFragment.Request(image, filename));
            if (fragment is null)
            {
                return NotFound();
            }

            return File(fragment.File, fragment.MimeType, filename);
        }
    }
}