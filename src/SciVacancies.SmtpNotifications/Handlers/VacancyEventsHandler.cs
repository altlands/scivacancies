//using SciVacancies.Domain.Enums;

using System;
using System.Linq;
using MediatR;
using NPoco;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.SmtpNotifications.SmtpNotificators;

namespace SciVacancies.SmtpNotifications.Handlers
{
    public class VacancyEventsHandler :
        INotificationHandler<VacancyInCommittee>,
        INotificationHandler<VacancyPublished>,
        INotificationHandler<VacancyPretenderSet>,
        INotificationHandler<VacancyOfferAcceptedByWinner>,
        INotificationHandler<VacancyOfferRejectedByWinner>,
        INotificationHandler<VacancyOfferAcceptedByPretender>,
        INotificationHandler<VacancyOfferRejectedByPretender>,
        INotificationHandler<VacancyClosed>,
        INotificationHandler<VacancyCancelled>
    {
        private readonly IDatabase _db;
        private readonly ISmtpNotificatorVacancyService _smtpNotificatorVacancyService;

        public VacancyEventsHandler(IDatabase db, ISmtpNotificatorVacancyService smtpNotificatorVacancyService)
        {
            _db = db;
            _smtpNotificatorVacancyService = smtpNotificatorVacancyService;
        }
        public void Handle(VacancyPublished msg)
        {
            VacancyStatusChangedSmtpNotificationForResearcher(msg.VacancyGuid);
        }
        public void Handle(VacancyInCommittee msg)
        {
            VacancyStatusChangedSmtpNotificationForOrganization(msg.VacancyGuid);
            VacancyStatusChangedSmtpNotificationForResearcher(msg.VacancyGuid);
        }
        public void Handle(VacancyPretenderSet msg)
        {

        }
        public void Handle(VacancyOfferAcceptedByWinner msg)
        {
            VacancyStatusChangedSmtpNotificationForOrganization(msg.VacancyGuid);
        }
        public void Handle(VacancyOfferRejectedByWinner msg)
        {
            VacancyStatusChangedSmtpNotificationForOrganization(msg.VacancyGuid);
        }
        public void Handle(VacancyOfferAcceptedByPretender msg)
        {
            VacancyStatusChangedSmtpNotificationForOrganization(msg.VacancyGuid);
        }
        public void Handle(VacancyOfferRejectedByPretender msg)
        {
            VacancyStatusChangedSmtpNotificationForOrganization(msg.VacancyGuid);
        }
        public void Handle(VacancyClosed msg)
        {
            VacancyStatusChangedSmtpNotificationForResearcher(msg.VacancyGuid);
        }

        public void Handle(VacancyCancelled msg)
        {
            VacancyStatusChangedSmtpNotificationForResearcher(msg.VacancyGuid);
        }


        private void VacancyStatusChangedSmtpNotificationForResearcher(Guid vacancyGuid)
        {
            var vacancy = _db.SingleOrDefaultById<Vacancy>(vacancyGuid);
            if (vacancy == null) return;
            var researcherGuids =
                _db.Fetch<Guid>(new Sql(
                    $"SELECT va.researcher_guid FROM res_vacancyapplications va WHERE va.vacancy_guid = @0", vacancyGuid));
            if (!researcherGuids.Any()) return;
            var researchers =
                _db.Fetch<Researcher>(new Sql($"SELECT * FROM res_researchers r WHERE r.guid IN (@0)",
                    researcherGuids));
            if (!researchers.Any()) return;

            foreach (var researcher in researchers)
                _smtpNotificatorVacancyService.SendVacancyStatusChangedForResearcher(vacancy, researcher);
        }


        private void VacancyStatusChangedSmtpNotificationForOrganization(Guid vacancyGuid)
        {
            var vacancy = _db.SingleOrDefaultById<Vacancy>(vacancyGuid);
            if (vacancy == null) return;
            var organization =
                _db.SingleOrDefaultById<Organization>(vacancy.organization_guid);

            _smtpNotificatorVacancyService.SendVacancyStatusChangedForOrganization(vacancy, organization);
        }


    }
}
