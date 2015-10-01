using System;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.SmtpNotifications.SmtpNotificators
{
    public interface ISmtpNotificatorSearchSubscriptionService
    {
        void SendCreated(Researcher researcher, Guid searchSubscriptionGuid, string title);
    }
}