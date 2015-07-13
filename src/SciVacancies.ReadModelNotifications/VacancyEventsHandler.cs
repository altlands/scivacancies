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
                    _db.Execute(new Sql($"INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyGuid, researcherGuid, msg.TimeStamp));
                }
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferAcceptedByWinner msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);

            VacancyApplication vacancyApplication = _db.Single<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0", vacancy.winner_vacancyapplication_guid));

            string title = "На ваш конкурс " + msg.VacancyGuid + " заявка-победитель " + vacancyApplication.guid + " подписывает контракт";
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"INSERT INTO org_notifications (guid, title, vacancyapplication_guid, organization_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, vacancyApplication.guid, msg.OrganizationGuid, msg.TimeStamp));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferRejectedByWinner msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);

            VacancyApplication vacancyApplication = _db.Single<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0", vacancy.winner_vacancyapplication_guid));

            string title = "На ваш конкурс " + msg.VacancyGuid + " заявка-победитель " + vacancyApplication.guid + " отказывается от контракта";
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"INSERT INTO org_notifications (guid, title, vacancyapplication_guid, organization_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, vacancyApplication.guid, msg.OrganizationGuid, msg.TimeStamp));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferAcceptedByPretender msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);

            VacancyApplication vacancyApplication = _db.Single<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0", vacancy.pretender_vacancyapplication_guid));

            string title = "На ваш конкурс " + msg.VacancyGuid + " заявка-претендент " + vacancyApplication.guid + " подписывает контракт";
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"INSERT INTO org_notifications (guid, title, vacancyapplication_guid, organization_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, vacancyApplication.guid, msg.OrganizationGuid, msg.TimeStamp));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferRejectedByPretender msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);

            VacancyApplication vacancyApplication = _db.Single<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0", vacancy.pretender_vacancyapplication_guid));

            string title = "На ваш конкурс " + msg.VacancyGuid + " заявка-претендент " + vacancyApplication.guid + " отказывается от контракта";
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"INSERT INTO org_notifications (guid, title, vacancyapplication_guid, organization_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, vacancyApplication.guid, msg.OrganizationGuid, msg.TimeStamp));
                transaction.Complete();
            }
        }
        public void Handle(VacancyClosed msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);

            List<Guid> researcherLooserGuids = _db.Fetch<Guid>(new Sql($"SELECT va.researcher_guid FROM res_vacancyapplications va WHERE va.vacancy_guid = @0 AND va.guid != @1 AND va.guid != @2", msg.VacancyGuid, vacancy.winner_vacancyapplication_guid, vacancy.pretender_vacancyapplication_guid));

            string looserTitle = "Ваша заявка на конкурс " + msg.VacancyGuid + " проиграла, конкурс закрыт";
            string title = "Конкурс: " + msg.VacancyGuid + " закрыт";
            using (var transaction = _db.GetTransaction())
            {
                foreach (Guid researcherGuid in researcherLooserGuids)
                {
                    _db.Execute(new Sql($"INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), looserTitle, msg.VacancyGuid, researcherGuid, msg.TimeStamp));
                }
                _db.Execute(new Sql($"INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyGuid, vacancy.winner_researcher_guid, msg.TimeStamp));
                _db.Execute(new Sql($"INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyGuid, vacancy.pretender_researcher_guid, msg.TimeStamp));
                transaction.Complete();
            }
        }
        public void Handle(VacancyCancelled msg)
        {
            List<Guid> researcherGuids = _db.Fetch<Guid>(new Sql($"SELECT va.researcher_guid FROM res_vacancyapplications va WHERE va.vacancy_guid = @0", msg.VacancyGuid));

            string title = "Ваша заявка на конкурс " + msg.VacancyGuid + " отменена. Причина: " + msg.Reason;
            using (var transaction = _db.GetTransaction())
            {
                foreach (Guid researcherGuid in researcherGuids)
                {
                    _db.Execute(new Sql($"INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyGuid, researcherGuid, msg.TimeStamp));
                }
                transaction.Complete();
            }
        }
    }
}
