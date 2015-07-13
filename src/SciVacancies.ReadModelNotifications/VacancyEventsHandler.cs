using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;
using NPoco;
using AutoMapper;

namespace SciVacancies.ReadModel.Notifications
{
    public class VacancyEventsHandler :
        INotificationHandler<VacancyInCommittee>,
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
        public void Handle(VacancyInCommittee msg)
        {
            List<Guid> researcherGuids = _db.Fetch<Guid>(new Sql($"SELECT va.researcher_guid FROM res_vacancyapplications va WHERE va.vacancy_guid = @0", msg.VacancyGuid));

            string title = "Ваша заявка на конкурс " + msg.VacancyGuid + " отправлена на комиссию";
            using (var transaction = _db.GetTransaction())
            {
                foreach (Guid researcherGuid in researcherGuids)
                {
                    _db.Insert(new Sql($"INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyGuid, researcherGuid, msg.TimeStamp));
                }
                transaction.Complete();
            }
        }
        public void Handle(VacancyPretenderSet msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE org_vacancies SET pretender_researcher_guid = @0, pretender_vacancyapplication_guid = @1, update_date = @2 WHERE guid = @3", msg.PretenderReasearcherGuid, msg.PretenderVacancyApplicationGuid, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferAcceptedByWinner msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE org_vacancies SET is_winner_accept = @0, status = @1, update_date = @2 WHERE guid = @3", true, VacancyStatus.OfferAccepted, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferRejectedByWinner msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE org_vacancies SET is_winner_accept = @0, update_date = @1 WHERE guid = @2", false, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferAcceptedByPretender msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE org_vacancies SET is_pretender_accept = @0, status = @1, update_date = @2 WHERE guid = @3", true, VacancyStatus.OfferAccepted, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferRejectedByPretender msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE org_vacancies SET is_pretender_accept = @0, status = @1, update_date = @2 WHERE guid = @3", false, VacancyStatus.OfferRejected, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyClosed msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE org_vacancies SET status = @0, update_date = @1 WHERE guid = @2", VacancyStatus.Closed, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyCancelled msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE org_vacancies SET status = @0, update_date = @1 WHERE guid = @2", VacancyStatus.Cancelled, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
    }
}
