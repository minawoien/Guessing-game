using System;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Images.Pipelines
{
    public class ResolveLayer
    {
        public record Request(int ImageId, int X, int Y) : IRequest<string>;

        public class Handler : IRequestHandler<Request, string>
        {
            private readonly GameContext _gameContext;

            public Handler(GameContext gameContext)
            {
                _gameContext = gameContext;
            }

            public async Task<string> Handle(Request request, CancellationToken cancellationToken)
            {
                var img = await _gameContext.Images.Include(i => i.Label)
                    .Include(i => i.Fragments)
                    .FirstOrDefaultAsync(o => o.Id == request.ImageId, cancellationToken: cancellationToken);
                return img.ResolveLayer(request.X, request.Y);
            }
        }
    }
}