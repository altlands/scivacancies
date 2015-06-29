using System;
using SciVacancies.Domain.DataModels;

namespace SciVacancies.Domain.Events
{
    public class ResearcherEventBase : EventBase
    {
        public Guid ResearcherGuid { get; set; }
    }

    public class ResearcherCreated : ResearcherEventBase
    {
        public ResearcherDataModel Data { get; set; }
    }

    public class ResearcherUpdated : ResearcherEventBase
    {
        public ResearcherDataModel Data { get; set; }
    }

    public class ResearcherRemoved : ResearcherEventBase
    {
    }
}