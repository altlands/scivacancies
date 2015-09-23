using System;
using System.Linq;
using MediatR;
using NPoco;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.SmtpNotifications.SmtpNotificators;

namespace SciVacancies.SmtpNotifications.Handlers
{
    public class VacancySagaEventHandlers :
        INotificationHandler<VacancySagaFirstInCommitteeNotificationSent>,
        INotificationHandler<VacancySagaSecondInCommitteeNotificationSent>,
        INotificationHandler<VacancySagaOfferResponseNotificationSentToWinner>,
        INotificationHandler<VacancySagaOfferResponseNotificationSentToPretender>
    {
        private readonly IDatabase _db;
        private readonly ISmtpNotificatorVacancyService _smtpNotificatorVacancyService;

        public VacancySagaEventHandlers(IDatabase db, ISmtpNotificatorVacancyService smtpNotificatorVacancyService)
        {
            _db = db;
            _smtpNotificatorVacancyService = smtpNotificatorVacancyService;
        }

        public void Handle(VacancySagaFirstInCommitteeNotificationSent msg)
        {
            var organization = _db.SingleOrDefaultById<Organization>(msg.OrganizationGuid);
            var vacancy = _db.SingleOrDefaultById<Vacancy>(msg.VacancyGuid);

            _smtpNotificatorVacancyService.SendFirstCommitteeNotificationToOrganization(organization, vacancy);
        }
        public void Handle(VacancySagaSecondInCommitteeNotificationSent msg)
        {
            var organization = _db.SingleOrDefaultById<Organization>(msg.OrganizationGuid);
            var vacancy = _db.SingleOrDefaultById<Vacancy>(msg.VacancyGuid);

            _smtpNotificatorVacancyService.SendSecondCommitteeNotificationToOrganization(organization, vacancy);
        }
        public void Handle(VacancySagaOfferResponseNotificationSentToWinner msg)
        {
            var researcher = _db.SingleOrDefaultById<Researcher>(msg.WinnerReasearcherGuid);

            _smtpNotificatorVacancyService.SendOfferResponseAwaitingNotificationToWinner(researcher, msg.WinnerVacancyApplicationGuid, msg.VacancyGuid);
        }
        public void Handle(VacancySagaOfferResponseNotificationSentToPretender msg)
        {
            var researcher = _db.SingleOrDefaultById<Researcher>(msg.PretenderReasearcherGuid);

            _smtpNotificatorVacancyService.SendOfferResponseAwaitingNotificationToPretender(researcher, msg.PretenderVacancyApplicationGuid, msg.VacancyGuid);
        }
    }
}
