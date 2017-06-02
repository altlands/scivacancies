using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;
using NPoco;

namespace SciVacancies.ReadModel.Notifications
{
    public class VacancyEventsHandler :
        INotificationHandler<VacancyInCommittee>,
        INotificationHandler<VacancyProlongedInCommittee>,
        INotificationHandler<VacancyInOfferResponseAwaitingFromWinner>,
        INotificationHandler<VacancyOfferAcceptedByWinner>,
        INotificationHandler<VacancyOfferRejectedByWinner>,
        INotificationHandler<VacancyInOfferResponseAwaitingFromPretender>,
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
            List<Guid> researcherGuids = _db.Fetch<Guid>(new Sql("SELECT va.researcher_guid FROM res_vacancyapplications va WHERE va.vacancy_guid = @0", msg.VacancyGuid));

            Vacancy vacancy = _db.SingleOrDefault<Vacancy>(new Sql("SELECT v.* FROM org_vacancies v WHERE v.guid = @0", msg.VacancyGuid));

            string title = "Ваша заявка на вакансию VAC " + vacancy.read_id + " отправлена на комиссию";
            using (var transaction = _db.GetTransaction())
            {
                foreach (Guid researcherGuid in researcherGuids)
                {
                    _db.Execute(new Sql("INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyGuid, researcherGuid, msg.TimeStamp));
                }
                transaction.Complete();
            }
        }
        public void Handle(VacancyProlongedInCommittee msg)
        {
            List<Guid> researcherGuids = _db.Fetch<Guid>(new Sql("SELECT va.researcher_guid FROM res_vacancyapplications va WHERE va.vacancy_guid = @0", msg.VacancyGuid));
            Vacancy vacancy = _db.SingleOrDefault<Vacancy>(new Sql("SELECT v.* FROM org_vacancies v WHERE v.guid = @0", msg.VacancyGuid));

            string title = "Комиссия по вакансии VAC " + vacancy.read_id + " продлена";
            using (var transaction = _db.GetTransaction())
            {
                foreach (Guid researcherGuid in researcherGuids)
                {
                    _db.Execute(new Sql("INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyGuid, researcherGuid, msg.TimeStamp));
                }
                transaction.Complete();
            }
        }
        public void Handle(VacancyInOfferResponseAwaitingFromWinner msg)
        {
            Vacancy vacancy = _db.SingleOrDefault<Vacancy>(new Sql("SELECT v.* FROM org_vacancies v WHERE v.guid = @0", msg.VacancyGuid));
            Researcher researcher = _db.SingleOrDefaultById<Researcher>(vacancy.winner_researcher_guid);
            VacancyApplication vacancyApplication = _db.SingleOrDefault<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0", vacancy.winner_vacancyapplication_guid));

            string title = $@"Уважаемый(-ая), {researcher.firstname} {researcher.patronymic} {researcher.secondname}, ваша ORD {vacancyApplication.read_id} победила в конкурсе на вакансию  VAC {vacancy.read_id}.
Вам необходимо в течение 30 календарных дней подтвердить или отвергуть предложение занять вакантную должность и заключить трудовой договор.";
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyGuid, researcher.guid, msg.TimeStamp));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferAcceptedByWinner msg)
        {
            Vacancy vacancy = _db.SingleOrDefault<Vacancy>(new Sql("SELECT v.* FROM org_vacancies v WHERE v.guid = @0", msg.VacancyGuid));

            VacancyApplication vacancyApplication = _db.SingleOrDefault<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0", vacancy.winner_vacancyapplication_guid));

            string title = $"На вашу вакансию VAC {vacancy.read_id} заявка-победитель ORD {vacancyApplication.read_id} подписывает контракт";
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"INSERT INTO org_notifications (guid, title, vacancyapplication_guid, organization_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, vacancyApplication.guid, msg.OrganizationGuid, msg.TimeStamp));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferRejectedByWinner msg)
        {
            Vacancy vacancy = _db.SingleOrDefault<Vacancy>(new Sql("SELECT v.* FROM org_vacancies v WHERE v.guid = @0", msg.VacancyGuid));

            VacancyApplication vacancyApplication = _db.SingleOrDefault<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0", vacancy.winner_vacancyapplication_guid));

            string title = "На вашу вакансию VAC " + vacancy.read_id + " заявка-победитель ORD " + vacancyApplication.read_id + " отказывается от контракта";
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql("INSERT INTO org_notifications (guid, title, vacancyapplication_guid, organization_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, vacancyApplication.guid, msg.OrganizationGuid, msg.TimeStamp));
                transaction.Complete();
            }
        }
        public void Handle(VacancyInOfferResponseAwaitingFromPretender msg)
        {
            Vacancy vacancy = _db.SingleOrDefault<Vacancy>(new Sql("SELECT v.* FROM org_vacancies v WHERE v.guid = @0", msg.VacancyGuid));
            Researcher researcher = _db.SingleOrDefaultById<Researcher>(vacancy.pretender_researcher_guid);
            VacancyApplication vacancyApplication = _db.SingleOrDefault<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0", vacancy.pretender_vacancyapplication_guid));

            string title = $@"Уважаемый(-ая), {researcher.firstname} {researcher.patronymic} {researcher.secondname}, ваша ORD {vacancyApplication.read_id} победила в конкурсе на вакансию  VAC {vacancy.read_id}.
Вам необходимо в течение 30 календарных дней подтвердить или отвергуть предложение занять вакантную должность и заключить трудовой договор.";
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql("INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyGuid, researcher.guid, msg.TimeStamp));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferAcceptedByPretender msg)
        {
            Vacancy vacancy = _db.SingleOrDefault<Vacancy>(new Sql("SELECT v.* FROM org_vacancies v WHERE v.guid = @0", msg.VacancyGuid));

            VacancyApplication vacancyApplication = _db.SingleOrDefault<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0", vacancy.pretender_vacancyapplication_guid));

            string title = "На ваш конкурс VAC " + vacancy.read_id + " заявка-претендент ORD " + vacancyApplication.read_id + " подписывает контракт";
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql("INSERT INTO org_notifications (guid, title, vacancyapplication_guid, organization_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, vacancyApplication.guid, msg.OrganizationGuid, msg.TimeStamp));
                transaction.Complete();
            }
        }
        public void Handle(VacancyOfferRejectedByPretender msg)
        {
            Vacancy vacancy = _db.SingleOrDefault<Vacancy>(new Sql("SELECT v.* FROM org_vacancies v WHERE v.guid = @0", msg.VacancyGuid));

            VacancyApplication vacancyApplication = _db.Single<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0", vacancy.pretender_vacancyapplication_guid));

            string title = "На вашу вакансию VAC " + vacancy.read_id + " заявка-претендент ORD" + vacancyApplication.read_id + " отказывается от контракта";
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql("INSERT INTO org_notifications (guid, title, vacancyapplication_guid, organization_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, vacancyApplication.guid, msg.OrganizationGuid, msg.TimeStamp));
                transaction.Complete();
            }
        }
        public void Handle(VacancyClosed msg)
        {
            Vacancy vacancy = _db.SingleOrDefault<Vacancy>(new Sql("SELECT v.* FROM org_vacancies v WHERE v.guid = @0", msg.VacancyGuid));

            List<Guid> researcherLooserGuids = _db.Fetch<Guid>(new Sql($"SELECT va.researcher_guid FROM res_vacancyapplications va WHERE va.vacancy_guid = @0 AND va.guid != @1 AND va.guid != @2", msg.VacancyGuid, vacancy.winner_vacancyapplication_guid, vacancy.pretender_vacancyapplication_guid));

            string looserTitle = "Ваша заявка на вакансию VAC " + vacancy.read_id + " проиграла, конкурс закрыт";
            string title = "Вакансия VAC " + vacancy.read_id + ": закрыта";
            using (var transaction = _db.GetTransaction())
            {
                foreach (Guid researcherGuid in researcherLooserGuids)
                {
                    _db.Execute(new Sql("INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), looserTitle, msg.VacancyGuid, researcherGuid, msg.TimeStamp));
                }
                _db.Execute(new Sql("INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyGuid, vacancy.winner_researcher_guid, msg.TimeStamp));
                if (vacancy.pretender_researcher_guid != Guid.Empty)
                    _db.Execute(new Sql("INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyGuid, vacancy.pretender_researcher_guid, msg.TimeStamp));
                transaction.Complete();
            }
        }
        public void Handle(VacancyCancelled msg)
        {
            Vacancy vacancy = _db.SingleOrDefault<Vacancy>(new Sql("SELECT v.* FROM org_vacancies v WHERE v.guid = @0", msg.VacancyGuid));

            List<Guid> researcherGuids = _db.Fetch<Guid>(new Sql("SELECT va.researcher_guid FROM res_vacancyapplications va WHERE va.vacancy_guid = @0", msg.VacancyGuid));

            string title = "Ваша заявка на вакансию VAC" + vacancy.read_id + " отменена. Причина: " + msg.Reason;
            using (var transaction = _db.GetTransaction())
            {
                foreach (Guid researcherGuid in researcherGuids)
                {
                    _db.Execute(new Sql("INSERT INTO res_notifications (guid, title, vacancy_guid, researcher_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyGuid, researcherGuid, msg.TimeStamp));
                }
                transaction.Complete();
            }
        }
    }
}
