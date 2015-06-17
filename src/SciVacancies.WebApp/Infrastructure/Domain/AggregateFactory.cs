using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CommonDomain;
using CommonDomain.Persistence;

namespace SciVacancies.WebApp.Infrastructure
{
    public class AggregateFactory:IConstructAggregates
    {
        public IAggregate Build(Type type,Guid id, IMemento snapshot)
        {
            return (IAggregate)Activator.CreateInstance(type);
        }
    }
}
