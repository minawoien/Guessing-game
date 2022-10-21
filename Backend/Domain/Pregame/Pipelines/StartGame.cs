using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Domain.Pregame.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Type = Backend.Domain.Game.Type;

namespace Backend.Domain.Pregame.Pipelines
{
    public class StartGame
    {
        public record Request(int UserId) : IRequest<PregameResponse<int>>;

        public class Handler : IRequestHandler<Request, PregameResponse<int>>
        {
            private readonly GameContext _db;

            public Handler(GameContext db)
            {
                _db = db;
            }

            public async Task<PregameResponse<int>> Handle(Request request, CancellationToken cancellationToken)
            {
                var lobby = await _db.Lobbies.Include(l => l.Players)
                    .FirstOrDefaultAsync(g => g.Players
                        .Any(p => p.UserId == request.UserId), cancellationToken);

                if (lobby is not null)
                {
                    if (lobby.Type == Type.MultiPlayer && lobby.Players.Count() < 2)
                    {
                        var error = new List<string>();
                        error.Add("Waiting for more players");
                        return new PregameResponse<int>(false, error.ToArray(), -1);
                    }

                    lobby.Events.Add(new GameStarted(lobby.Id, lobby.Players, lobby.Type));
                    await _db.RunEvents();
                    return new PregameResponse<int>(true, Array.Empty<string>(), lobby.Id);
                }

                var err = new List<string>();
                err.Add("Game was not found");
                return new PregameResponse<int>(false, err.ToArray(), -1);
            }
        }
    }
}