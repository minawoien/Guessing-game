using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Domain.Game.Events;
using MediatR;

namespace Backend.Domain.Pregame.Handlers
{
    public class GameCreatedHandler : INotificationHandler<GameCreated>
    {
        private readonly GameContext _gameContext;

        public GameCreatedHandler(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        //Used for updating gameId in lobby when game is created.
        public async Task Handle(GameCreated notification, CancellationToken cancellationToken)
        {
            var lobby = await _gameContext.Lobbies.FindAsync(notification.LobbyId);
            lobby.GameId = notification.GameId;
            await _gameContext.SaveChangesAsync(cancellationToken);
        }
    }
}