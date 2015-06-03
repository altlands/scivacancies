using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Interfaces
{
    public interface IResearcherService
    {
        Guid CreateResearcher(ResearcherDataModel data);
        void UpdateResearcher(Guid researcherGuid, ResearcherDataModel data);
        void RemoveResearcher(Guid researcherGuid);

        Guid CreateVacancyApplicationTemplate(Guid researcherGuid, Guid vacancyGuid);
        void ApplyToVacancy(Guid researcherGuid, Guid vacancyApplicationGuid);
    }
}
