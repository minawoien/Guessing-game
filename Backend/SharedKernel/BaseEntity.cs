using System.Collections.Generic;

namespace Backend.SharedKernel
{
    public abstract class BaseEntity
    {
        public List<BaseDomainEvent> Events = new();
    }
}