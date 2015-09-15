using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
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
        INotificationHandler<VacancyInOfferResponseAwaitingFromWinner>,
        INotificationHandler<VacancyOfferAcceptedByWinner>,
        INotificationHandler<VacancyOfferRejectedByWinner>,
        INotificationHandler<VacancyInOfferResponseAwaitingFromPretender>,
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
                if (vacancy.criterias != null)
                {
                    foreach (VacancyCriteria vc in vacancy.criterias)
                    {
                        if (vc.guid == Guid.Empty) vc.guid = Guid.NewGuid();
                        vc.vacancy_guid = vacancy.guid;
                        _db.Insert(vc);
                    }
                }
                if (vacancy.attachments != null)
                {
                    foreach (VacancyAttachment at in vacancy.attachments)
                    {
                        if (at.guid == Guid.Empty) at.guid = Guid.NewGuid();
                        at.vacancy_guid = vacancy.guid;
                        _db.Insert(at);
                    }
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
                _db.Update(updatedVacancy);
                //TODO - без удаления
                _db.Execute(new Sql($"DELETE FROM org_vacancycriterias WHERE vacancy_guid = @0", msg.VacancyGuid));
                _db.Execute(new Sql($"DELETE FROM org_vacancy_attachments WHERE vacancy_guid = @0", msg.VacancyGuid));
                if (updatedVacancy.criterias != null)
                {
                    foreach (VacancyCriteria vc in updatedVacancy.criterias)
                    {
                        if (vc.guid == Guid.Empty) vc.guid = Guid.NewGuid();
                        vc.vacancy_guid = vacancy.guid;
                        _db.Insert(vc);
                    }
                }
                if (updatedVacancy.attachments != null)
                {
                    foreach (VacancyAttachment at in updatedVacancy.attachments)
                    {
                        if (at.guid == Guid.Empty) at.guid = Guid.NewGuid();
                        at.vacancy_guid = vacancy.guid;
                        _db.Insert(at);
                    }
                }

                transaction.Complete();
            }
        }
        public void Handle(VacancyRemoved msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET status = @0, update_date = @1 WHERE guid = @2", VacancyStatus.Removed, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyPublished msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET publish_date = @0, status = @1, update_date = @2 WHERE guid = @3", msg.TimeStamp, VacancyStatus.Published, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyInCommittee msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET committee_date = @0, status = @1, update_date = @2 WHERE guid = @3", msg.TimeStamp, VacancyStatus.InCommittee, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyWinnerSet msg)
        {
            using (var transaction = _db.GetTransaction())
            {

                _db.Execute(new Sql($"UPDATE org_vacancies SET winner_researcher_guid = @0, winner_vacancyapplication_guid = @1, close_reason=@2, update_date = @3 WHERE guid = @4", msg.WinnerReasearcherGuid, msg.WinnerVacancyApplicationGuid, msg.Reason, msg.TimeStamp, msg.VacancyGuid));

                if (msg.Attachments != null)
                {
                var attachments = Mapper.Map<List<VacancyAttachment>>(msg.Attachments);
                    foreach (var at in attachments)
                    {
                        if (at.guid == Guid.Empty) at.guid = Guid.NewGuid();
                        at.vacancy_guid = msg.VacancyGuid;
                        _db.Insert(at);
                    }
                }

                transaction.Complete();
            }
        }
        public void Handle(VacancyPretenderSet msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET pretender_researcher_guid = @0, pretender_vacancyapplication_guid = @1, update_date = @2 WHERE guid = @3", msg.PretenderReasearcherGuid, msg.PretenderVacancyApplicationGuid, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyInOfferResponseAwaitingFromWinner msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET winner_request_date = @0, awaiting_date = @0, status = @1, update_date = @0 WHERE guid = @2", msg.TimeStamp, VacancyStatus.OfferResponseAwaitingFromWinner, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferAcceptedByWinner msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET is_winner_accept = @0, status = @1, winner_response_date = @2, update_date = @2 WHERE guid = @3", true, VacancyStatus.OfferAcceptedByWinner, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferRejectedByWinner msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET is_winner_accept = @0, status = @1, winner_response_date = @2, update_date = @2 WHERE guid = @3", false, VacancyStatus.OfferRejectedByWinner, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyInOfferResponseAwaitingFromPretender msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET status = @0, pretender_request_date = @1, update_date = @1 WHERE guid = @2", VacancyStatus.OfferResponseAwaitingFromPretender, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferAcceptedByPretender msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET is_pretender_accept = @0, status = @1, pretender_response_date = @2, update_date = @2 WHERE guid = @3", true, VacancyStatus.OfferAcceptedByPretender, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferRejectedByPretender msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET is_pretender_accept = @0, status = @1, pretender_response_date = @2, update_date = @2 WHERE guid = @3", false, VacancyStatus.OfferRejectedByPretender, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyClosed msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET announcement_date = @0, status = @1, update_date = @2 WHERE guid = @3", msg.TimeStamp, VacancyStatus.Closed, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyCancelled msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_vacancies SET announcement_date = @0, cancel_reason = @1, status = @2, update_date = @3 WHERE guid = @4", msg.TimeStamp, msg.Reason, VacancyStatus.Cancelled, msg.TimeStamp, msg.VacancyGuid));
                transaction.Complete();
            }
        }

        public void Handle(VacancyAddedToFavorites msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"INSERT INTO res_favoritevacancies (guid, vacancy_guid, researcher_guid) VALUES(@0, @1, @2)", Guid.NewGuid(), msg.VacancyGuid, msg.ResearcherGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyRemovedFromFavorites msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"DELETE FROM res_favoritevacancies WHERE researcher_guid = @0 AND vacancy_guid = @1", msg.ResearcherGuid, msg.VacancyGuid));
                transaction.Complete();
            }
        }
    }
}
