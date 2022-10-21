using System.Collections.Generic;

namespace Backend.Domain.Result
{
    public record RecentGameDTO(List<string> UserNames, string GameType, string StartTime);
}