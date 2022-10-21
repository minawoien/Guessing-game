using System.Threading;
using System.Threading.Tasks;
using Backend.Domain.Game.Services;
using MediatR;

namespace Backend.Domain.Game.Pipelines
{
    public class UnlockWithOracle
    {
        public record Request(int GameId) : IRequest<GameServiceDataResponse<string>>;

        public class Handler : IRequestHandler<Request, GameServiceDataResponse<string>>
        {
            private readonly IGameService _gameService;

            public Handler(IGameService gameService)
            {
                _gameService = gameService;
            }

            public async Task<GameServiceDataResponse<string>> Handle(Request request,
                CancellationToken cancellationToken)
            {
                return await _gameService.UnlockFragmentWithOracle(request.GameId, false);
            }
        }
    }
}