using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Type = Backend.Domain.Game.Type;

namespace Backend.Domain.Pregame.Pipelines
{
    public class GetLobbyByType
    {
        public record Request(int Type) : IRequest<PregameResponse<List<LobbyDTO>>>;

        public class Handler : IRequestHandler<Request, PregameResponse<List<LobbyDTO>>>
        {
            private readonly GameContext _db;

            public Handler(GameContext db)
            {
                _db = db;
            }


            public async Task<PregameResponse<List<LobbyDTO>>> Handle(Request request,
                CancellationToken cancellationToken)
            {
                var lobbyType = (Type) request.Type;

                var gameLobbies = await _db.Lobbies
                    .Include(p => p.Players)
                    .Where(t => t.Type == lobbyType).ToArrayAsync(cancellationToken: cancellationToken);


                var dtos = new List<LobbyDTO>();
                if (gameLobbies.Length == 0)
                {
                    List<string> err = new();
                    err.Add("No gameLobby of this type exists");
                    return new PregameResponse<List<LobbyDTO>>(false, err.ToArray(), dtos);
                }

                foreach (var game in gameLobbies)
                {
                    var players = game.Players.Select(p => p.Username);
                    var hostRole = game.Players.Select(p => p.Role).FirstOrDefault();
                    var dto = new LobbyDTO(game.Id, players.ToList(), (int) game.Type, (int) hostRole, game.GameId);
                    dtos.Add(dto);
                }

                return new PregameResponse<List<LobbyDTO>>(true, Array.Empty<string>(), dtos);
            }
        }
    }
}