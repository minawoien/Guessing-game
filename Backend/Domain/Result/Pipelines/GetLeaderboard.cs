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
    public class GetLeaderboard
    {
        public record Request : IRequest<ResultServiceResponse<List<ResultDTO>>>;

        public class Handler : IRequestHandler<Request, ResultServiceResponse<List<ResultDTO>>>
        {
            private readonly GameContext _db;

            public Handler(GameContext db)
            {
                _db = db;
            }

            public async Task<ResultServiceResponse<List<ResultDTO>>> Handle(Request request,
                CancellationToken cancellationToken)
            {
                var numEntries = 20;
                var results = await _db.Results
                    .OrderByDescending(r => r.Score)
                    .Take(numEntries).ToListAsync();

                if (!results.Any())
                {
                    List<string> err = new();
                    err.Add("No games have been played");
                    return new ResultServiceResponse<List<ResultDTO>>(false, err.ToArray(), null);
                }

                var dtos = new List<ResultDTO>();
                foreach (var result in results)
                {
                    var dto = new ResultDTO(result.UserName, result.Score);
                    dtos.Add(dto);
                }

                return new ResultServiceResponse<List<ResultDTO>>(true, Array.Empty<string>(), dtos);
            }
        }
    }
}