using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;
using NPoco;

namespace SciVacancies.ReadModel.Handlers
{
    public abstract class EventBaseHandler<T> : INotificationHandler<T> where T : EventBase
    {
        protected readonly IDatabase _db;
        protected EventBaseHandler(IDatabase db)
        {
            _db = db;
        }
        public abstract void Handle(T msg);
    }
}
