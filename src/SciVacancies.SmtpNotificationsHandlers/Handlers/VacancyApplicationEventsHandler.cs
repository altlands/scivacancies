using MediatR;
using NPoco;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.Services.SmtpNotificators;

namespace SciVacancies.SmtpNotificationsHandlers.Handlers
{
    public class VacancyApplicationEventsHandler :
        INotificationHandler<VacancyApplicationApplied>
    {
        private readonly IDatabase _db;
        private readonly ISmtpNotificatorVacancyService _smtpNotificatorVacancyService;

        public VacancyApplicationEventsHandler(IDatabase db, ISmtpNotificatorVacancyService smtpNotificatorVacancyService)
        {
            _db = db;
            _smtpNotificatorVacancyService = smtpNotificatorVacancyService;
        }

        public void Handle(VacancyApplicationApplied msg)
        {
            var vacancyapplication = _db.SingleOrDefaultById<VacancyApplication>(msg.VacancyApplicationGuid);
            if (vacancyapplication == null) return;
            var vacancy = _db.SingleOrDefaultById<Vacancy>(msg.VacancyGuid);
            if (vacancy == null) return;
            var researcher = _db.SingleOrDefaultById<Researcher>(vacancyapplication.researcher_guid);
            if (researcher == null) return;
            var organization = _db.SingleOrDefaultById<Organization>(vacancy.organization_guid);
            if (organization == null) return;

            _smtpNotificatorVacancyService.SendVacancyApplicationAppliedForOrganization(organization, vacancy, vacancyapplication);
            _smtpNotificatorVacancyService.SendVacancyApplicationAppliedForResearcher(researcher, vacancy, vacancyapplication);
        }


    }
}