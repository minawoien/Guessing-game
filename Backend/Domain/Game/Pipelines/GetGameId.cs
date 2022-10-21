using System.Threading;
using System.Threading.Tasks;
using Backend.Domain.Game.Services;
using MediatR;

namespace Backend.Domain.Game.Pipelines
{
    public class GetGameId
    {
        public record Request(int UserId) : IRequest<int>;

        public class Handler : IRequestHandler<Request, int>
        {
            private readonly IGameService _gameService;

            public Handler(IGameService gameService)
            {
                _gameService = gameService;
            }


            public async Task<int> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _gameService.GetGameId(request.UserId);
            }
        }
    }
}