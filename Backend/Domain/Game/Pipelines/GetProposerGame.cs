using System.Threading;
using System.Threading.Tasks;
using Backend.Domain.Game.Services;
using MediatR;

namespace Backend.Domain.Game.Pipelines
{
    public class GetProposerGame
    {
        public record Request(int UserId) : IRequest<GameServiceDataResponse<(int GameId, int ImageId)>>;

        public class Handler : IRequestHandler<Request, GameServiceDataResponse<(int GameId, int ImageId)>>
        {
            private readonly IGameService _gameService;

            public Handler(IGameService gameService)
            {
                _gameService = gameService;
            }

            public async Task<GameServiceDataResponse<(int GameId, int ImageId)>> Handle(Request request,
                CancellationToken cancellationToken)
            {
                return await _gameService.GetProposerGame(request.UserId);
            }
        }
    }
}