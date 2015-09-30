using MediatR;
using NPoco;
using SciVacancies.Domain.Events;
using SciVacancies.SmtpNotifications.SmtpNotificators;

namespace SciVacancies.SmtpNotifications.Handlers
{
    public class ResearcherEventsHandler :
        INotificationHandler<ResearcherCreated>
    {
        private readonly IDatabase _db;
        private readonly ISmtpNotificatorAccountService _smtpNotificatorAccountService;

        public ResearcherEventsHandler(IDatabase db, ISmtpNotificatorAccountService smtpNotificatorAccountService)
        {
            _db = db;
            _smtpNotificatorAccountService = smtpNotificatorAccountService;
        }

        public void Handle(ResearcherCreated msg)
        {
            //var researcher = _db.SingleOrDefaultById<Researcher>(msg.ResearcherGuid);
            //if (researcher == null) return;

            //send email notification
            _smtpNotificatorAccountService.SendUserRegistered(msg.Data.FullName, msg.Data.Email, msg.Data.UserName);
        }
    }
}
