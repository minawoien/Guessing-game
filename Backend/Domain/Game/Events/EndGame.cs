using Backend.SharedKernel;

namespace Backend.Domain.Game.Events
{
    public record EndGame(ResultDTO Results) : BaseDomainEvent;
}