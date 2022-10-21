using Backend.SharedKernel;

namespace Backend.Domain.Game.Events
{
    public record JoinedGame(int UserId) : BaseDomainEvent;
}