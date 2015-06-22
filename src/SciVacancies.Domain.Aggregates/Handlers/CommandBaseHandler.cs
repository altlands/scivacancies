using SciVacancies.Domain.Aggregates.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;
using CommonDomain.Persistence;

namespace SciVacancies.Domain.Aggregates.Handlers
{
    //public abstract class CommandBaseHandler<TRequest, M> : IRequestHandler<TRequest, M> where TRequest : CommandBase
    //{
    //    protected readonly IRepository _repository;

    //    protected CommandBaseHandler(IRepository rep)
    //    {
    //        _repository = rep;
    //    }
    //    public abstract M Handle(TRequest cmd);
    //}
}

//namespace SciVacancies.ReadModel.Handlers
//{
//    public abstract class EventBaseHandler<T> : INotificationHandler<T> where T : EventBase
//    {
//        protected readonly IDatabase _db;
//        protected EventBaseHandler(IDatabase db)
//        {
//            _db = db;
//        }
//        public abstract void Handle(T msg);
//    }
//}