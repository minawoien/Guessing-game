using System.Threading;
using System.Threading.Tasks;
using Backend.Domain.Game.Services;
using MediatR;

namespace Backend.Domain.Game.Pipelines
{
    public class UnlockWithProposer
    {
        public record Request(int GameId, string Fragment) : IRequest<GameServiceResponse>;

        public class Handler : IRequestHandler<Request, GameServiceResponse>
        {
            private readonly IGameService _gameService;

            public Handler(IGameService gameService)
            {
                _gameService = gameService;
            }

            public async Task<GameServiceResponse> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _gameService.UnlockFragmentWithProposer(request.GameId, request.Fragment);
            }
        }
    }
}