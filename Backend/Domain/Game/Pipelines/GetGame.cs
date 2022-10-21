using System.Threading;
using System.Threading.Tasks;
using Backend.Domain.Game.Services;
using MediatR;

namespace Backend.Domain.Game.Pipelines
{
    public class GetGame
    {
        public record Request(int UserId) : IRequest<GameDTO>;

        public class Handler : IRequestHandler<Request, GameDTO>
        {
            private readonly IGameService _gameService;

            public Handler(IGameService gameService)
            {
                _gameService = gameService;
            }


            public async Task<GameDTO> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _gameService.GetGame(request.UserId);
            }
        }
    }
}