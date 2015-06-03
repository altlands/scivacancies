using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
{
    public class VacancyApplicationEventBase : EventBase
    {
        public VacancyApplicationEventBase() : base() { }

        public Guid VacancyApplicationGuid { get; set; }
        public Guid VacancyGuid { get; set; }
        public Guid ResearcherGuid { get; set; }
    }
    public class VacancyApplicationCreated : VacancyApplicationEventBase
    {
        public VacancyApplicationCreated() : base() { }


    }
    public class VacancyApplicationApplied : VacancyApplicationEventBase
    {
        public VacancyApplicationApplied() : base() { }


    }
}
