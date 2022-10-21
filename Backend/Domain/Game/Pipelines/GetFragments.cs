using System.Threading;
using System.Threading.Tasks;
using Backend.Domain.Game.Services;
using MediatR;

namespace Backend.Domain.Game.Pipelines
{
    public class GetFragments
    {
        public record Request(int GameId, int UserId) : IRequest<ImagePathResponse>;

        public class Handler : IRequestHandler<Request, ImagePathResponse>
        {
            private readonly IGameService _gameService;

            public Handler(IGameService gameService)
            {
                _gameService = gameService;
            }

            public async Task<ImagePathResponse> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _gameService.GetFragments(request.GameId, request.UserId);
            }
        }
    }
}