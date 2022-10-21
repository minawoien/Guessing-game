using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Domain.Auth;
using Backend.Domain.Game;
using Backend.Domain.Images;
using Backend.Domain.Pregame;
using Backend.Domain.Result;
using Backend.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class GameContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public GameContext(DbContextOptions configuration, IMediator mediator) : base(configuration)
        {
            _mediator = mediator;
        }


        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<Lobby> Lobbies { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<GameResult> Results { get; set; } = null!;
        public DbSet<TeamResult> TeamResults { get; set; } = null!;
        public DbSet<RecentGame> RecentGames { get; set; } = null!;
        private readonly IMediator _mediator;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public async Task RunEvents()
        {
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();
            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    await _mediator.Publish(domainEvent, cancellationToken: new CancellationToken());
                }
            }
        }
    }
}