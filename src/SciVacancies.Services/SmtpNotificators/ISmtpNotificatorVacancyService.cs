using System;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.Services.SmtpNotificators
{
    public interface ISmtpNotificatorVacancyService
    {
        void SendVacancyApplicationAppliedForOrganization(Organization organization, Vacancy vacancy, VacancyApplication vacancyApplication);
        void SendVacancyApplicationAppliedForResearcher(Researcher researcher, Vacancy vacancy, VacancyApplication vacancyApplication);
        void SendVacancyStatusChangedForOrganization(Vacancy vacancy, Organization organization);
        void SendVacancyStatusChangedForResearcher(Vacancy vacancy, Researcher researcher);
        void SendWinnerSet(Researcher researcher, Guid applicationGuid, Guid vacancyGuid);
    }
}