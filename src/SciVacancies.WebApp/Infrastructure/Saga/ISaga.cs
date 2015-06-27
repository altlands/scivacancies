using System.Collections;
using MediatR;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public interface ISaga
    {
        string Id { get; }

        int Version { get; }

        ICollection GetUncommittedEvents();

        void ClearUncommittedEvents();

        ICollection GetUndispatchedMessages();

        void ClearUndispatchedMessages();

        void Transition<TMessage>(TMessage message) where TMessage : INotification;
    }    
}
