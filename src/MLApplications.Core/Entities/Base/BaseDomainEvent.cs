using MediatR;
using System;

namespace MLApplications.Core.Entities.Base
{
    /// <summary>
    ///     Base Domain Event. 
    ///     Abstract class as no event should just be base.
    /// </summary>
    public abstract class BaseDomainEvent : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}
