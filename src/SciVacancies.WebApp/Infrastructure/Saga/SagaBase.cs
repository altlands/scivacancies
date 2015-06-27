using System.Collections;
using System.Collections.Generic;
using MediatR;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class SagaBase : ISaga
    {
        public string Id { get; protected set; }
        public int Version { get; protected set; }

        private readonly ICollection<INotification> _uncommitted = new LinkedList<INotification>();

        private readonly ICollection<INotification> _undispatched = new LinkedList<INotification>();

        public ICollection GetUncommittedEvents()
        {
            return _uncommitted as ICollection;
        }

        public void ClearUncommittedEvents()
        {
            _uncommitted.Clear();
        }

        public ICollection GetUndispatchedMessages()
        {
            return _undispatched as ICollection;
        }

        public void ClearUndispatchedMessages()
        {
            _uncommitted.Clear();
        }

        public void Transition<TMessage>(TMessage message) where TMessage : INotification
        {
            var saga = this as ISagaMessageHandler<TMessage>;
            if (saga != null)
            {
                saga.Handle(message);
                _uncommitted.Add(message);
                Version++;
            }            
        }
    }
}
