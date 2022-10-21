using System.Threading;
using System.Threading.Tasks;
using Backend.Domain.Game.Services;
using MediatR;

namespace Backend.Domain.Game.Pipelines
{
    public class RegisterGuess
    {
        public record Request(string guess, int UserId) : IRequest<GameServiceDataResponse<string>>;

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
                return await _gameService.RegisterGuess(request.guess, request.UserId);
            }
        }
    }
}