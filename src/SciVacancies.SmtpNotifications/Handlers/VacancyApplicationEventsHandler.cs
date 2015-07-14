using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;

using MediatR;

namespace SciVacancies.SmtpNotifications.Handlers
{
    public class VacancyApplicationEventsHandler :
        INotificationHandler<VacancyApplicationApplied>
    {
        //private readonly IDatabase _db;

        //public VacancyApplicationEventsHandler(IDatabase db)
        //{
        //    _db = db;
        //}
        public void Handle(VacancyApplicationApplied msg)
        {

        }
    }
}