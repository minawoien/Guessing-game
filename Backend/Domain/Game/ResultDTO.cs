using System.Collections.Generic;

namespace Backend.Domain.Game
{
    public record ResultDTO(int Score,
        string StartTime, Type Type, List<string> Players, string Winner
    );
}