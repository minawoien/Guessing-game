using Backend.SharedKernel;

namespace Backend.Domain.Game.Events
{
    public record GameCreated(int GameId, int LobbyId) : BaseDomainEvent;
}