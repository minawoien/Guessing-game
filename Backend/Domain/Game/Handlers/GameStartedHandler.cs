using System.Threading;
using System.Threading.Tasks;
using Backend.Domain.Game.Services;
using Backend.Domain.Pregame.Events;
using MediatR;

namespace Backend.Domain.Game.Handlers
{
    public class GameStartedHandler : INotificationHandler<GameStarted>
    {
        private readonly IGameService _gameService;

        public GameStartedHandler(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Handle(GameStarted notification, CancellationToken cancellationToken)
        {
            await _gameService.StartGame(notification.LobbyId, notification.Players, notification.Type);
        }
    }
}