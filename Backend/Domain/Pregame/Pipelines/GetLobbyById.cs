using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Pregame.Pipelines
{
    public class GetLobbyById
    {
        public record Request(int userId) : IRequest<PregameResponse<LobbyDTO>>;

        public class Handler : IRequestHandler<Request, PregameResponse<LobbyDTO>>
        {
            private readonly GameContext _db;

            public Handler(GameContext db)
            {
                _db = db;
            }


            public async Task<PregameResponse<LobbyDTO>> Handle(Request request, CancellationToken cancellationToken)
            {
                var gameLobby = await _db.Lobbies
                    .Include(p => p.Players)
                    .FirstOrDefaultAsync(g => g.Players.Any(p => p.UserId == request.userId), cancellationToken);


                if (gameLobby == null)
                {
                    List<string> err = new();
                    err.Add("No gameLobby with this id exist");
                    return new PregameResponse<LobbyDTO>(false, err.ToArray(), null);
                }

                var players = gameLobby.Players.Select(p => p.Username);
                var hostRole = gameLobby.Players.Select(p => p.Role).First();

                return new PregameResponse<LobbyDTO>(true, Array.Empty<string>(),
                    new LobbyDTO(gameLobby.Id, players.ToList(), (int) gameLobby.Type, (int) hostRole,
                        gameLobby.GameId));
            }
        }
    }
}