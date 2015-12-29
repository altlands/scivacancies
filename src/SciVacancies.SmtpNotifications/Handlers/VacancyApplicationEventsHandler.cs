using MediatR;
using NPoco;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.SmtpNotifications.SmtpNotificators;

namespace SciVacancies.SmtpNotifications.Handlers
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
            VacancyApplication vacancyapplication = _db.SingleOrDefault<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0", msg.VacancyApplicationGuid));

            if (vacancyapplication == null) return;
            Vacancy vacancy = _db.SingleOrDefault<Vacancy>(new Sql($"SELECT v.* FROM org_vacancies v WHERE v.guid = @0", msg.VacancyGuid));

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