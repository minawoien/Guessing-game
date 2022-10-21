using System.Threading;
using System.Threading.Tasks;
using Backend.Domain.Game.Services;
using MediatR;

namespace Backend.Domain.Game.Pipelines
{
    public class SetWaitingStatus
    {
        public record Request(int GameId) : IRequest;

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IGameService _gameService;

            public Handler(IGameService gameService)
            {
                _gameService = gameService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                await _gameService.SetWaitingStatus(request.GameId);
                return Unit.Value;
            }
        }
    }
}