using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Pregame.Pipelines
{
    public class QuitLobby
    {
        public record Request(int UserId) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly GameContext _db;

            public Handler(GameContext db)
            {
                _db = db;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var gameLobby = await _db.Lobbies
                    .Include(p => p.Players)
                    .FirstOrDefaultAsync(g => g.Players.Any(p => p.UserId == request.UserId), cancellationToken);

                var player = gameLobby.Players.FirstOrDefault(p => p.UserId == request.UserId);

                _db.Remove(player);
                gameLobby.Players.Remove(player);
                if (gameLobby.Players.Count == 0)
                {
                    _db.Remove(gameLobby);
                }

                await _db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}