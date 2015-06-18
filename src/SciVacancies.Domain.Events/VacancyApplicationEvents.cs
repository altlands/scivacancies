using SciVacancies.Domain.DataModels;

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

        public VacancyApplicationDataModel Data { get; set; }
    }
    public class VacancyApplicationUpdated:VacancyApplicationEventBase
    {
        public VacancyApplicationUpdated() : base() { }

        public VacancyApplicationDataModel Data { get; set; }
    }
    public class VacancyApplicationRemoved:VacancyApplicationEventBase
    {
        public VacancyApplicationRemoved() : base() { }
    }
    public class VacancyApplicationApplied : VacancyApplicationEventBase
    {
        public VacancyApplicationApplied() : base() { }
    }
    public class VacancyApplicationCancelled:VacancyApplicationEventBase
    {
        public VacancyApplicationCancelled() : base() { }
    }
    public class VacancyApplicationWon:VacancyApplicationEventBase
    {
        public VacancyApplicationWon() : base() { }
    }
    public class VacancyApplicationPretended:VacancyApplicationEventBase
    {
        public VacancyApplicationPretended() : base() { }
    }
    public class VacancyApplicationLost:VacancyApplicationEventBase
    {
        public VacancyApplicationLost() : base() { }
    }
}
