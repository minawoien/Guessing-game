using System.Collections.Generic;

namespace Backend.Domain.Result
{
    public record TeamResultDTO(List<string> UserNames, int Score);
}