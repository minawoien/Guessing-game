using System.Collections.Generic;
using Backend.Domain.Game;
using Backend.SharedKernel;

namespace Backend.Domain.Pregame.Events
{
    public record GameStarted(int LobbyId, List<LobbyPlayer> Players, Type Type) : BaseDomainEvent;
}