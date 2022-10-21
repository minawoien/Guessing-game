using System.Collections.Generic;

namespace Backend.Domain.Game
{
    public record GameDTO(int Id, int Status, int UnlockedFragments, List<string> Guesses, int Role, string ImageLabel,
        int Type, string Winner);
}