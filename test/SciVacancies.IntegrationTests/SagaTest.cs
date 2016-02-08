using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonDomain.Core;
using CommonDomain.Persistence.EventStore;
using MediatR;
using SciVacancies.Domain.Events;
using SciVacancies.WebApp.Infrastructure.Saga;
using Xunit;

namespace SciVacancies.IntegrationTests
{
    public class SagaTest
    {
        [Fact]
        public void Test1()
        {
            var factory = new SagaFactory();
            var saga = factory.Build(typeof (SagaX), "sagax");

            saga.Transition(new VacancyApplicationCreated() as INotification);
            saga.Transition(new VacancyApplicationUpdated() as INotification);                    
        }
    }
}
