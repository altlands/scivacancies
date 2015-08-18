using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using NPoco;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.SmtpNotificationsHandlers.SmtpNotificators;

namespace SciVacancies.SmtpNotificationsHandlers.Handlers
{
    public class ResearcherEventsHandler :
        INotificationHandler<ResearcherCreated>
    {
        private readonly IDatabase _db;

        public ResearcherEventsHandler(IDatabase db)
        {
            _db = db;
        }

        public void Handle(ResearcherCreated msg)
        {
            var researcher = _db.SingleOrDefaultById<Researcher>(msg.ResearcherGuid);
            if (researcher == null) return;

            //send email notification
            var registerSmtpNotificator = new SmtpNotificatorUserRegistered();
            registerSmtpNotificator.Send(msg.Data.FullName, msg.Data.Email, msg.Data.ExtraEmail);
        }
    }
}
