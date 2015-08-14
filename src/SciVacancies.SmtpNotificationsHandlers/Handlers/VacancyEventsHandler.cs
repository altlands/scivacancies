//using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using MediatR;
using NPoco;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.SmtpNotificationsHandlers.SmtpNotificators;

namespace SciVacancies.SmtpNotificationsHandlers.Handlers
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

        public VacancyEventsHandler(IDatabase db)
        {
            _db = db;


        }
        public void Handle(VacancyPublished msg)
        {
            VacancyStatusChangedSmtpNotification(msg.VacancyGuid);
        }
        public void Handle(VacancyInCommittee msg)
        {
            VacancyStatusChangedSmtpNotification(msg.VacancyGuid);
        }
        public void Handle(VacancyPretenderSet msg)
        {

        }
        public void Handle(VacancyOfferAcceptedByWinner msg)
        {

        }
        public void Handle(VacancyOfferRejectedByWinner msg)
        {

        }
        public void Handle(VacancyOfferAcceptedByPretender msg)
        {

        }
        public void Handle(VacancyOfferRejectedByPretender msg)
        {

        }
        public void Handle(VacancyClosed msg)
        {
            VacancyStatusChangedSmtpNotification(msg.VacancyGuid);
        }

        public void Handle(VacancyCancelled msg)
        {
            VacancyStatusChangedSmtpNotification(msg.VacancyGuid);
        }



        private void VacancyStatusChangedSmtpNotification(Guid vacancyGuid)
        {
            var vacancy = _db.SingleOrDefaultById<Vacancy>(vacancyGuid);
            var researcherGuids =
                _db.Fetch<Guid>(new Sql(
                    $"SELECT va.researcher_guid FROM res_vacancyapplications va WHERE va.vacancy_guid = @0", vacancyGuid));
            var researchers =
                _db.Fetch<Researcher>(new Sql($"SELECT * FROM res_researchers r WHERE r.guid IN (@0)", researcherGuids));

            var smtpNotificatorVacancyStatusChanged = new SmtpNotificatorVacancyStatusChanged();
            foreach (var researcher in researchers)
            {
                smtpNotificatorVacancyStatusChanged.Send(vacancy, researcher);
            }
        }


    }
}
