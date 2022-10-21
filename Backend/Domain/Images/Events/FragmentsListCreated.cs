using System.Collections.Generic;
using Backend.SharedKernel;

namespace Backend.Domain.Images.Events
{
    public record FragmentsListCreated
        (int GameId, int ImageId, string Label, List<string> FragmentList) : BaseDomainEvent;
}