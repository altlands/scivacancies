using System;
using System.Net;
using System.Net.Mail;

namespace SciVacancies.SmtpNotifications
{
    public class SmtpEmailService : IEmailService, IDisposable
    {
        private readonly string _login;
        private readonly string _password;
        private readonly Lazy<SmtpClient> _smtpClient;

        public SmtpEmailService(string login, string password, string host, int port, bool enableSsl, bool useDefaultCredentials)
        {
            _login = login;
            _password = password;

            _smtpClient = new Lazy<SmtpClient>(()=> new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = useDefaultCredentials,
                Credentials = new NetworkCredential(_login, _password)
            });
        }

        public virtual void SendEmail(MailMessage message)
        {
            _smtpClient.Value.Send(message);
        }

        public void Dispose()
        {
            if (_smtpClient.IsValueCreated)
            {
                _smtpClient.Value.Dispose();
            }
        }
    }
}
