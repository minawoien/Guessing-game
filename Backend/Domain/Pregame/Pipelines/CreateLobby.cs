using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Pregame.Pipelines
{
    public class CreateLobby
    {
        public record Request
            (int UserId, string UserName, LobbyData LobbyData) : IRequest<PregameResponse<LobbyStatus>>;

        public class Handler : IRequestHandler<Request, PregameResponse<LobbyStatus>>
        {
            private readonly GameContext _db;

            public Handler(GameContext db)
            {
                _db = db;
            }

            public async Task<PregameResponse<LobbyStatus>> Handle(Request request, CancellationToken cancellationToken)
            {
                var gameLobby = await _db.Lobbies
                    .Include(g => g.Players)
                    .FirstOrDefaultAsync(c => c.Id == request.LobbyData.Id, cancellationToken);
                if (gameLobby is null)
                {
                    gameLobby = new Lobby(request.LobbyData.Type);
                    _db.Add(gameLobby);
                }

                var existingLobby = await _db.Lobbies
                    .Include(p => p.Players)
                    .FirstOrDefaultAsync(g => g.Players
                        .Any(p => p.UserId == request.UserId), cancellationToken);
                if (existingLobby is not null)
                {
                    var player = existingLobby.Players
                        .FirstOrDefault(p => p.UserId == request.UserId);
                    if (player != null)
                    {
                        _db.Remove(player);
                        existingLobby.Players.Remove(player);
                    }

                    if (existingLobby.Players.Count == 0)
                    {
                        _db.Lobbies.Remove(existingLobby);
                    }
                }

                gameLobby.AddUsers(request.UserId, request.LobbyData.Role, request.UserName);
                await _db.SaveChangesAsync(cancellationToken);

                return new PregameResponse<LobbyStatus>(true, Array.Empty<string>(),
                    new LobbyStatus(gameLobby.Id, gameLobby.GameId));
            }
        }
    }
}