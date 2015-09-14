using System;
using MediatR;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySagaCreated : INotification
    {
        public Guid SagaGuid { get; set; }
    }
    public class VacancySagaSwitchedInCommittee : INotification
    {
        public Guid SagaGuid { get; set; }
    }
}
