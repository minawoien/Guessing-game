using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Domain.Game;
using Backend.Domain.Game.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Result.Handlers
{
    public class EndGameHandler : INotificationHandler<EndGame>
    {
        private readonly GameContext _db;

        public EndGameHandler(GameContext db)
        {
            _db = db;
        }

        public async Task Handle(EndGame notification, CancellationToken cancellationToken)
        {
            // Add to Recent Game
            var recentGame = new RecentGame(notification.Results.Type, notification.Results.StartTime);
            _db.RecentGames.Add(recentGame);

            foreach (var player in notification.Results.Players)
            {
                recentGame.AddUsers(player);
            }

            await _db.SaveChangesAsync(cancellationToken);

            if (notification.Results.Winner == "No winner")
            {
                return;
            }

            // Check if type is TwoPlayer
            if (notification.Results.Type == Type.TwoPlayer)
            {
                var teamResult = new TeamResult(notification.Results.Score);
                foreach (var player in notification.Results.Players)
                {
                    teamResult.AddUsers(player);
                }

                _db.TeamResults.Add(teamResult);
                await _db.SaveChangesAsync(cancellationToken);
                return;
            }

            var existingResult = await _db.Results
                .Where(g => g.UserName == notification.Results.Winner)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (existingResult is null)
            {
                var result = new GameResult(notification.Results.Winner, notification.Results.Score);
                _db.Results.Add(result);
                await _db.SaveChangesAsync(cancellationToken);
                return;
            }

            existingResult.UpdateScore(notification.Results.Score);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}