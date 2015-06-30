using MediatR;
using NPoco;
using System;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.ReadModel.EventHandlers
{
    public class VacancyApplicationEventsHandler : 
        INotificationHandler<VacancyApplicationCreated>,
        INotificationHandler<VacancyApplicationUpdated>,
        INotificationHandler<VacancyApplicationRemoved>,
        INotificationHandler<VacancyApplicationApplied>,
        INotificationHandler<VacancyApplicationCancelled>,
        INotificationHandler<VacancyApplicationWon>,
        INotificationHandler<VacancyApplicationLost>
    {
        private readonly IDatabase _db;

        public VacancyApplicationEventsHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(VacancyApplicationCreated msg)
        {
            VacancyApplication vacancyApplication = new VacancyApplication()
            {
                Guid = msg.VacancyApplicationGuid,
                VacancyGuid = msg.VacancyGuid,
                ResearcherGuid = msg.ResearcherGuid,
                CreationDate = msg.TimeStamp,
                Status = VacancyApplicationStatus.InProcess
            };

            _db.Insert(vacancyApplication);
        }

        public void Handle(VacancyApplicationUpdated msg)
        {
            VacancyApplication vacancyApplication = _db.SingleById<VacancyApplication>(msg.VacancyApplicationGuid);
            //TODO
            vacancyApplication.UpdateDate = msg.TimeStamp;

            _db.Update(vacancyApplication);
        }

        public void Handle(VacancyApplicationRemoved msg)
        {
            VacancyApplication vacancyApplication = _db.SingleById<VacancyApplication>(msg.VacancyApplicationGuid);

            vacancyApplication.Status = VacancyApplicationStatus.Removed;

            _db.Update(vacancyApplication);
            //_db.Delete<VacancyApplication>(msg.VacancyApplicationGuid);
        }

        public void Handle(VacancyApplicationApplied msg)
        {
            VacancyApplication vacancyApplication = _db.SingleById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.Status = VacancyApplicationStatus.Applied;
            vacancyApplication.UpdateDate = msg.TimeStamp;

            _db.Update(vacancyApplication);

            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);

            Notification notification = new Notification()
            {
                Guid = Guid.NewGuid(),
                OrganizationGuid = vacancy.OrganizationGuid,
                CreationDate = msg.TimeStamp,
                Status = NotificationStatus.Created,
                Title = "На ваш конкурс" + msg.VacancyGuid + " подана новая заявка "
            };

            _db.Insert(notification);
        }

        public void Handle(VacancyApplicationCancelled msg)
        {
            VacancyApplication vacancyApplication = _db.SingleById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.Status = VacancyApplicationStatus.Cancelled;
            vacancyApplication.UpdateDate = msg.TimeStamp;

            _db.Update(vacancyApplication);

            Notification notification = new Notification()
            {
                Guid = Guid.NewGuid(),
                ResearcherGuid = msg.ResearcherGuid,
                CreationDate = msg.TimeStamp,
                Status = NotificationStatus.Created,
                Title = "Ваша заявка" + msg.VacancyApplicationGuid + " отклонена"
            };

            _db.Insert(notification);
        }

        public void Handle(VacancyApplicationWon msg)
        {
            VacancyApplication vacancyApplication = _db.SingleById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.Status = VacancyApplicationStatus.Won;
            vacancyApplication.UpdateDate = msg.TimeStamp;

            _db.Update(vacancyApplication);

            Notification notification = new Notification()
            {
                Guid = Guid.NewGuid(),
                ResearcherGuid = msg.ResearcherGuid,
                CreationDate = msg.TimeStamp,
                Status = NotificationStatus.Created,
                Title = "Ваша заявка" + msg.VacancyApplicationGuid + " выйграла конкурс"
            };

            _db.Insert(notification);
        }

        public void Handle(VacancyApplicationPretended msg)
        {
            VacancyApplication vacancyApplication = _db.SingleById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.Status = VacancyApplicationStatus.Pretended;
            vacancyApplication.UpdateDate = msg.TimeStamp;

            _db.Update(vacancyApplication);

            Notification notification = new Notification()
            {
                Guid = Guid.NewGuid(),
                ResearcherGuid = msg.ResearcherGuid,
                CreationDate = msg.TimeStamp,
                Status = NotificationStatus.Created,
                Title = "Ваша заявка" + msg.VacancyApplicationGuid + " получила второе место в конкурсе. Ожидайте чуда."
            };

            _db.Insert(notification);
        }

        public void Handle(VacancyApplicationLost msg)
        {
            VacancyApplication vacancyApplication = _db.SingleById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.Status = VacancyApplicationStatus.Lost;
            vacancyApplication.UpdateDate = msg.TimeStamp;

            _db.Update(vacancyApplication);

            Notification notification = new Notification()
            {
                Guid = Guid.NewGuid(),
                ResearcherGuid = msg.ResearcherGuid,
                CreationDate = msg.TimeStamp,
                Status = NotificationStatus.Created,
                Title = "Ваша заявка" + msg.VacancyApplicationGuid + " проиграла конкурс"
            };

            _db.Insert(notification);
        }
    }
}