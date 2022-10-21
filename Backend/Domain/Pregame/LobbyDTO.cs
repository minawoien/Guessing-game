using System.Collections.Generic;

namespace Backend.Domain.Pregame
{
    public record LobbyDTO(int Id, List<string> UserNames, int GameType, int HostRole, int GameId);
}