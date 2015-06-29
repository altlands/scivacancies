using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;
using MediatR;

namespace SciVacancies.ReadModel.EventHandlers
{
    public class VacancyApplicationCreatedHandler : INotificationHandler<VacancyApplicationCreated>
    {
        private readonly IDatabase _db;

        public VacancyApplicationCreatedHandler(IDatabase db)
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
    }
    public class VacancyApplicationUpdatedHandler : INotificationHandler<VacancyApplicationUpdated>
    {
        private readonly IDatabase _db;

        public VacancyApplicationUpdatedHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(VacancyApplicationUpdated msg)
        {
            VacancyApplication vacancyApplication = _db.SingleById<VacancyApplication>(msg.VacancyApplicationGuid);
            //TODO
            vacancyApplication.UpdateDate = msg.TimeStamp;

            _db.Update(vacancyApplication);
        }
    }
    public class VacancyApplicationRemovedHandler : INotificationHandler<VacancyApplicationRemoved>
    {
        private readonly IDatabase _db;

        public VacancyApplicationRemovedHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(VacancyApplicationRemoved msg)
        {
            VacancyApplication vacancyApplication = _db.SingleById<VacancyApplication>(msg.VacancyApplicationGuid);

            vacancyApplication.Status = VacancyApplicationStatus.Removed;

            _db.Update(vacancyApplication);
            //_db.Delete<VacancyApplication>(msg.VacancyApplicationGuid);
        }
    }
    public class VacancyApplicationAppliedHandler : INotificationHandler<VacancyApplicationApplied>
    {
        private readonly IDatabase _db;

        public VacancyApplicationAppliedHandler(IDatabase db)
        {
            _db = db;
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
                CreationdDate = msg.TimeStamp,
                Status = NotificationStatus.Created,
                Title = "На ваш конкурс" + msg.VacancyGuid + " подана новая заявка "
            };

            _db.Insert(notification);
        }
    }
    public class VacancyApplicationCancelledHandler : INotificationHandler<VacancyApplicationCancelled>
    {
        private readonly IDatabase _db;

        public VacancyApplicationCancelledHandler(IDatabase db)
        {
            _db = db;
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
                CreationdDate = msg.TimeStamp,
                Status = NotificationStatus.Created,
                Title = "Ваша заявка" + msg.VacancyApplicationGuid + " отклонена"
            };

            _db.Insert(notification);
        }
    }
    public class VacancyApplicationWonHandler : INotificationHandler<VacancyApplicationWon>
    {
        private readonly IDatabase _db;

        public VacancyApplicationWonHandler(IDatabase db)
        {
            _db = db;
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
                CreationdDate = msg.TimeStamp,
                Status = NotificationStatus.Created,
                Title = "Ваша заявка" + msg.VacancyApplicationGuid + " выйграла конкурс"
            };

            _db.Insert(notification);
        }
    }
    public class VacancyApplicationPretendedHandler : INotificationHandler<VacancyApplicationPretended>
    {
        private readonly IDatabase _db;

        public VacancyApplicationPretendedHandler(IDatabase db)
        {
            _db = db;
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
                CreationdDate = msg.TimeStamp,
                Status = NotificationStatus.Created,
                Title = "Ваша заявка" + msg.VacancyApplicationGuid + " получила второе место в конкурсе"
            };

            _db.Insert(notification);
        }
    }
    public class VacancyApplicationLostHandler : INotificationHandler<VacancyApplicationLost>
    {
        private readonly IDatabase _db;

        public VacancyApplicationLostHandler(IDatabase db)
        {
            _db = db;
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
                CreationdDate = msg.TimeStamp,
                Status = NotificationStatus.Created,
                Title = "Ваша заявка" + msg.VacancyApplicationGuid + " проиграла конкурс"
            };

            _db.Insert(notification);
        }
    }
}