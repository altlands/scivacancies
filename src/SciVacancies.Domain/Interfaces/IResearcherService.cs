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

        int AddVacancyToFavorites(Guid researcherGuid, Guid vacancyGuid);
        int RemoveVacancyFromFavorites(Guid researcherGuid, Guid vacancyGuid);

        Guid CreateVacancyApplicationTemplate(Guid researcherGuid, Guid vacancyGuid);
        void UpdateVacancyApplicationTemplate(Guid vacancyApplicationGuid, VacancyApplicationDataModel data);
        void RemoveVacancyApplicationTemplate(Guid vacancyApplicationGuid);
        void ApplyToVacancy(Guid researcherGuid, Guid vacancyApplicationGuid);
    }
}
