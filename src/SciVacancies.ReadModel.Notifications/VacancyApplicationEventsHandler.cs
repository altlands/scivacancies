using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;

using MediatR;
using NPoco;

namespace SciVacancies.ReadModel.Notifications
{
    public class VacancyApplicationEventsHandler :
        INotificationHandler<VacancyApplicationApplied>
    {
        private readonly IDatabase _db;

        public VacancyApplicationEventsHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(VacancyApplicationApplied msg)
        {
            Vacancy vacancy = _db.SingleOrDefault<Vacancy>(new Sql($"SELECT v.* FROM org_vacancies v WHERE v.guid = @0", msg.VacancyGuid));

            VacancyApplication vacancyApplication = _db.SingleOrDefault<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0", msg.VacancyApplicationGuid));

            string title = "Подана заявка ORD " + vacancyApplication.read_id + " на вакансию VAC "+ vacancy.read_id;
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"INSERT INTO org_notifications (guid, title, vacancyapplication_guid, organization_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyApplicationGuid, vacancy.organization_guid, msg.TimeStamp));
                transaction.Complete();
            }
        }
    }
}