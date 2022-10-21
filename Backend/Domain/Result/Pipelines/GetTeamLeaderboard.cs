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
    public class GetTeamLeaderboard
    {
        public record Request : IRequest<ResultServiceResponse<List<TeamResultDTO>>>;

        public class Handler : IRequestHandler<Request, ResultServiceResponse<List<TeamResultDTO>>>
        {
            private readonly GameContext _db;

            public Handler(GameContext db)
            {
                _db = db;
            }

            public async Task<ResultServiceResponse<List<TeamResultDTO>>> Handle(Request request,
                CancellationToken cancellationToken)
            {
                var numEntries = 20;
                var results = await _db.TeamResults
                    .Include(p => p.Players)
                    .OrderByDescending(r => r.Score)
                    .Take(numEntries).ToListAsync(cancellationToken);

                if (!results.Any())
                {
                    List<string> err = new();
                    err.Add("No games have been played");
                    return new ResultServiceResponse<List<TeamResultDTO>>(false, err.ToArray(), null);
                }

                var dtos = new List<TeamResultDTO>();
                foreach (var result in results)
                {
                    var players = result.Players.Select(p => p.UserName);
                    var dto = new TeamResultDTO(players.ToList(), result.Score);
                    dtos.Add(dto);
                }


                return new ResultServiceResponse<List<TeamResultDTO>>(true, Array.Empty<string>(), dtos);
            }
        }
    }
}