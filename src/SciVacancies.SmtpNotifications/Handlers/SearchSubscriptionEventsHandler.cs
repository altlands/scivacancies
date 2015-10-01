using MediatR;
using NPoco;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.SmtpNotifications.SmtpNotificators;

namespace SciVacancies.SmtpNotifications.Handlers
{
    public class SearchSubscriptionEventsHandler :
        INotificationHandler<SearchSubscriptionCreated>
    {
        private readonly IDatabase _db;
        private readonly ISmtpNotificatorSearchSubscriptionService _smtpNotificatorSearchSubscriptionService;

        public SearchSubscriptionEventsHandler(ISmtpNotificatorSearchSubscriptionService smtpNotificatorSearchSubscriptionService, IDatabase db)
        {
            _db = db;
            _smtpNotificatorSearchSubscriptionService = smtpNotificatorSearchSubscriptionService;
        }

        public void Handle(SearchSubscriptionCreated msg)
        {
            var researcher = _db.SingleOrDefaultById<Researcher>(msg.ResearcherGuid);
            if (researcher == null) return;

            //send email notification
            _smtpNotificatorSearchSubscriptionService.SendCreated(researcher, msg.SearchSubscriptionGuid, msg.Data.Title);
        }
    }
}
