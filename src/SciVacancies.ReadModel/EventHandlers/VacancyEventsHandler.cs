using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;

using MediatR;
using NPoco;
using AutoMapper;

namespace SciVacancies.ReadModel.EventHandlers
{
    public class VacancyEventsHandler :
        INotificationHandler<VacancyCreated>,
        INotificationHandler<VacancyUpdated>,
        INotificationHandler<VacancyRemoved>,
        INotificationHandler<VacancyPublished>,
        INotificationHandler<VacancyInCommittee>,
        INotificationHandler<VacancyWinnerSet>,
        INotificationHandler<VacancyPretenderSet>,
        INotificationHandler<VacancyOfferAcceptedByWinner>,
        INotificationHandler<VacancyOfferRejectedByWinner>,
        INotificationHandler<VacancyOfferAcceptedByPretender>,
        INotificationHandler<VacancyOfferRejectedByPretender>,
        INotificationHandler<VacancyClosed>,
        INotificationHandler<VacancyCancelled>,

        INotificationHandler<VacancyAddedToFavorites>,
        INotificationHandler<VacancyRemovedFromFavorites>
    {
        private readonly IDatabase _db;

        public VacancyEventsHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(VacancyCreated msg)
        {
            Vacancy vacancy = Mapper.Map<Vacancy>(msg);

            using (var transaction = _db.GetTransaction())
            {
                _db.Insert(vacancy);
                foreach(VacancyCriteria vc in vacancy.criterias)
                {
                    vc.vacancy_guid = vacancy.guid;
                    _db.Insert(vc);
                }
                transaction.Complete();
            }
        }
        public void Handle(VacancyUpdated msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);

            Vacancy updatedVacancy = Mapper.Map<Vacancy>(msg);
            updatedVacancy.creation_date = vacancy.creation_date;

            using (var transaction = _db.GetTransaction())
            {
                _db.Update(vacancy);
                _db.Delete(new Sql($"DELETE FROM org_vacancycriterias WHERE vacancy_guid = @0", msg.VacancyGuid));
                foreach (VacancyCriteria vc in vacancy.criterias)
                {
                    vc.vacancy_guid = vacancy.guid;
                    _db.Insert(vc);
                }
                transaction.Complete();
            }
        }
        public void Handle(VacancyRemoved msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE org_vacancies SET status = @0, update_date = @1 WHERE guid = @2", VacancyStatus.Removed, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyPublished msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET status = @0, update_date = @1 WHERE guid = @2", VacancyStatus.Published, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyInCommittee msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE org_vacancies SET status = @0, update_date = @1 WHERE guid = @2", VacancyStatus.InCommittee, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyWinnerSet msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE org_vacancies SET winner_researcher_guid = @0, winner_vacancyapplication_guid = @1, update_date = @2 WHERE guid = @3", msg.WinnerReasearcherGuid, msg.WinnerVacancyApplicationGuid, msg.TimeStamp, msg.VacancyGuid));
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

        public void Handle(VacancyAddedToFavorites msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Insert(new Sql($"INSERT INTO res_favoritevacancies (guid, vacancy_guid, researcher_guid) VALUES(@0, @1, @2)", Guid.NewGuid(), msg.VacancyGuid, msg.ResearcherGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyRemovedFromFavorites msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Delete(new Sql($"DELETE FROM res_favoritevacancies WHERE researcher_guid = @0 AND vacancy_guid = @1", msg.ResearcherGuid, msg.VacancyGuid));
                transaction.Complete();
            }
        }
    }
}
