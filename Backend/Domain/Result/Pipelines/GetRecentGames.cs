using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Result.Pipelines
{
    public class GetRecentGames
    {
        public record Request : IRequest<ResultServiceResponse<List<RecentGameDTO>>>;

        public class Handler : IRequestHandler<Request, ResultServiceResponse<List<RecentGameDTO>>>
        {
            private readonly GameContext _db;

            public Handler(GameContext db)
            {
                _db = db;
            }

            public async Task<ResultServiceResponse<List<RecentGameDTO>>> Handle(Request request,
                CancellationToken cancellationToken)
            {
                var numEntries = 20;
                var recentGames = await _db.RecentGames
                    .Include(p => p.Players)
                    .OrderByDescending(c => c.Id)
                    .Take(numEntries).ToListAsync(cancellationToken);
                if (!recentGames.Any())
                {
                    List<string> err = new();
                    err.Add("No games have been played");
                    return new ResultServiceResponse<List<RecentGameDTO>>(false, err.ToArray(), null);
                }

                var dtos = new List<RecentGameDTO>();
                foreach (var game in recentGames)
                {
                    var players = game.Players.Select(p => p.UserName);
                    var dto = new RecentGameDTO(players.ToList(), game.Type, game.StartTime);
                    dtos.Add(dto);
                }

                return new ResultServiceResponse<List<RecentGameDTO>>(true, Array.Empty<string>(), dtos);
            }
        }
    }
}