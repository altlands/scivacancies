using MediatR;
using NPoco;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.SmtpNotificationsHandlers.SmtpNotificators;

namespace SciVacancies.SmtpNotificationsHandlers.Handlers
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
            var vacancyapplication = _db.SingleOrDefaultById<VacancyApplication>(msg.VacancyApplicationGuid);
            var vacancy = _db.SingleOrDefaultById<Vacancy>(msg.VacancyGuid);
            var organization = _db.SingleOrDefaultById<Organization>(vacancy.organization_guid);

            var smtpNotificatorVacancyApplicationApplied = new SmtpNotificatorVacancyApplicationApplied();
            smtpNotificatorVacancyApplicationApplied.Send(organization, vacancy, vacancyapplication);
        }
    }
}