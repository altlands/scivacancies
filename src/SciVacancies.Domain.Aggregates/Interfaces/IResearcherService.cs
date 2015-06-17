using SciVacancies.Domain.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Aggregates.Interfaces
{
    public interface IResearcherService
    {
        Guid CreateResearcher(ResearcherDataModel data);
        void UpdateResearcher(Guid researcherGuid, ResearcherDataModel data);
        void RemoveResearcher(Guid researcherGuid);

        int AddVacancyToFavorites(Guid researcherGuid, Guid vacancyGuid);
        int RemoveVacancyFromFavorites(Guid researcherGuid, Guid vacancyGuid);

        Guid CreateSearchSubscription(Guid researcherGuid, SearchSubscriptionDataModel data);
        void ActivateSearchSubscription(Guid researcherGuid, Guid searchSubscriptionGuid);
        void CancelSearchSubscription(Guid researcherGuid, Guid searchSubscriptionGuid);
        void RemoveSearchSubscription(Guid researcherGuid, Guid searchSubscriptionGuid);

        Guid CreateVacancyApplicationTemplate(Guid researcherGuid, Guid vacancyGuid);
        void UpdateVacancyApplicationTemplate(Guid researcherGuid, Guid vacancyApplicationGuid, VacancyApplicationDataModel data);
        void RemoveVacancyApplicationTemplate(Guid researcherGuid, Guid vacancyApplicationGuid);
        void ApplyToVacancy(Guid researcherGuid, Guid vacancyApplicationGuid);
    }
}
