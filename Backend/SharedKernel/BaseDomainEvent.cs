using System;
using MediatR;

namespace Backend.SharedKernel
{
    public abstract record BaseDomainEvent : INotification
    {
        public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.UtcNow;
    }
}