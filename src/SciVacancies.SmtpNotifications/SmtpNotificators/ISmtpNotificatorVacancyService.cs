﻿using SciVacancies.ReadModel.Core;

using System;

namespace SciVacancies.SmtpNotifications.SmtpNotificators
{
    public interface ISmtpNotificatorVacancyService
    {
        void SendVacancyApplicationAppliedForOrganization(Organization organization, Vacancy vacancy, VacancyApplication vacancyApplication);
        void SendVacancyApplicationAppliedForResearcher(Researcher researcher, Vacancy vacancy, VacancyApplication vacancyApplication);
        void SendVacancyStatusChangedForOrganization(Vacancy vacancy, Organization organization);
        void SendVacancyStatusChangedForResearcher(Vacancy vacancy, Researcher researcher);
        void SendVacancyProlongedForResearcher(Vacancy vacancy, Researcher researcher);
        void SendWinnerSet(Researcher researcher, Guid applicationGuid, Guid vacancyGuid);

        void SendFirstCommitteeNotificationToOrganization(Organization organization, Vacancy vacancy);
        void SendSecondCommitteeNotificationToOrganization(Organization organization, Vacancy vacancy);
        void SendOfferResponseAwaitingNotificationToWinner(Researcher researcher, Guid applicationGuid, Guid vacancyGuid);
        void SendOfferResponseAwaitingNotificationToPretender(Researcher researcher, Guid applicationGuid, Guid vacancyGuid);
        void SendOfferRejectedByPretender(Vacancy vacancy, Organization organization);
        void SendOfferRejectedByWinner(Vacancy vacancy, Organization organization);
        void SendOfferAcceptedByPretender(Vacancy vacancy, Organization organization);
        void SendOfferAcceptedByWinner(Vacancy vacancy, Organization organization);
    }
}