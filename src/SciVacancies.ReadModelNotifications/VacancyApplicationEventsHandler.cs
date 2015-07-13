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
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);

            string title = "На конкурс " + msg.VacancyGuid + " подана новая заявка " + msg.VacancyApplicationGuid;
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"INSERT INTO org_notifications (guid, title, vacancyapplication_guid, organization_guid, creation_date) VALUES(@0, @1, @2, @3, @4)", Guid.NewGuid(), title, msg.VacancyApplicationGuid, vacancy.organization_guid, msg.TimeStamp));
                transaction.Complete();
            }
        }
    }
}