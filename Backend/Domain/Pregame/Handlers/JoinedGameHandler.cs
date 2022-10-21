using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Domain.Game.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Pregame.Handlers
{
    public class JoinedGameHandler : INotificationHandler<JoinedGame>
    {
        private readonly GameContext _gameContext;

        public JoinedGameHandler(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public async Task Handle(JoinedGame notification, CancellationToken cancellationToken)
        {
            var lobby = await _gameContext.Lobbies
                .Include(l => l.Players)
                .FirstOrDefaultAsync(l => l.Players.Any(p => p.UserId == notification.UserId), cancellationToken);
            var player = lobby.Players.FirstOrDefault(p => p.UserId == notification.UserId);
            if (player != null)
            {
                _gameContext.Remove(player);
                lobby.Players.Remove(player);
            }

            if (lobby.Players.Count == 0)
            {
                _gameContext.Lobbies.Remove(lobby);
            }

            await _gameContext.SaveChangesAsync(cancellationToken);
        }
    }
}