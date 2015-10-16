using SciVacancies.ReadModel.Core;

using System;

namespace SciVacancies.SmtpNotifications.SmtpNotificators
{
    public interface ISmtpNotificatorSearchSubscriptionService
    {
        void SendCreated(Researcher researcher, Guid searchSubscriptionGuid, string title);
    }
}