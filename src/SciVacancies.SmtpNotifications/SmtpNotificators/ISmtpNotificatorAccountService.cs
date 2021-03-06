﻿using SciVacancies.ReadModel.Core;

namespace SciVacancies.SmtpNotifications.SmtpNotificators
{
    public interface ISmtpNotificatorAccountService
    {
        void SendPasswordRestore(Researcher researcher, string username, string mailTo, string token);
        void SendUserActivation(Researcher researcher, string username, string mailTo, string token);
        void SendUserRegistered(string fullName, string mailTo, string login);
    }
}