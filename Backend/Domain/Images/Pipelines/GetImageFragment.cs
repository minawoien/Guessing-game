using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Images.Pipelines
{
    public class GetImageFragment
    {
        public record Request(int ImageId, string Filename) : IRequest<ImageFragment>;

        public class Handler : IRequestHandler<Request, ImageFragment>
        {
            private readonly GameContext _gameContext;

            public Handler(GameContext gameContext)
            {
                _gameContext = gameContext;
            }

            public async Task<ImageFragment> Handle(Request request, CancellationToken cancellationToken)
            {
                var image = await _gameContext.Images.Include(i => i.Label).Include(i => i.Fragments)
                    .FirstOrDefaultAsync(o => o.Id == request.ImageId, cancellationToken: cancellationToken);
                ImageFragment imageFragment = image.Fragments.FirstOrDefault(f => f.FileName == request.Filename);
                return imageFragment;
            }
        }
    }
}