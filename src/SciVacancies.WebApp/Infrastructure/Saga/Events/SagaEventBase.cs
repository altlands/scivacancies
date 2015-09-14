using System;
using MediatR;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public abstract class SagaEventBase : INotification
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
    }
}
