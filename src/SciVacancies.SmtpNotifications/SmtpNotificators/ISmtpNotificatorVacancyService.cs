using SciVacancies.ReadModel.Core;

using System;
using SciVacancies.Domain.Enums;

namespace SciVacancies.SmtpNotifications.SmtpNotificators
{
    public interface ISmtpNotificatorVacancyService
    {
        void SendVacancyApplicationAppliedForOrganization(Organization organization, Vacancy vacancy, VacancyApplication vacancyApplication);
        void SendVacancyApplicationAppliedForResearcher(Researcher researcher, Vacancy vacancy, VacancyApplication vacancyApplication);
        void SendVacancyStatusChangedForOrganization(Vacancy vacancy, Organization organization, VacancyStatus status);
        void SendVacancyStatusChangedForResearcher(Vacancy vacancy, Researcher researcher, VacancyStatus status);
        void SendVacancyProlongedForResearcher(Vacancy vacancy, Researcher researcher);
        void SendWinnerSet(Researcher researcher, Guid applicationGuid, Guid vacancyGuid);
        /// <summary>
        /// Первое напоминание: нужно выбрать победителя
        /// </summary>
        /// <param name="organization"></param>
        /// <param name="vacancy"></param>
        void SendFirstCommitteeNotificationToOrganization(Organization organization, Vacancy vacancy);
        /// <summary>
        /// Второе напоминание: нужно выбрать победителя
        /// </summary>
        /// <param name="organization"></param>
        /// <param name="vacancy"></param>
        void SendSecondCommitteeNotificationToOrganization(Organization organization, Vacancy vacancy);
        void SendOfferResponseAwaitingNotificationToWinner(Researcher researcher, Guid applicationGuid, Guid vacancyGuid);
        void SendOfferResponseAwaitingNotificationToPretender(Researcher researcher, Guid applicationGuid, Guid vacancyGuid);
        void SendOfferRejectedByPretender(Vacancy vacancy, Organization organization);
        void SendOfferRejectedByWinner(Vacancy vacancy, Organization organization);
        void SendOfferAcceptedByPretender(Vacancy vacancy, Organization organization);
        void SendOfferAcceptedByWinner(Vacancy vacancy, Organization organization);
    }
}