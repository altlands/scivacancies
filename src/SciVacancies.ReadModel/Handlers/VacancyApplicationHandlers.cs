using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Handlers
{
    public class VacancyApplicationCreatedHandler : EventBaseHandler<VacancyApplicationCreated>
    {
        public VacancyApplicationCreatedHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyApplicationCreated msg)
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
    public class VacancyApplicationUpdatedHandler : EventBaseHandler<VacancyApplicationUpdated>
    {
        public VacancyApplicationUpdatedHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyApplicationUpdated msg)
        {
            VacancyApplication vacancyApplication = _db.SingleById<VacancyApplication>(msg.VacancyApplicationGuid);
            //TODO
            vacancyApplication.UpdateDate = msg.TimeStamp;

            _db.Update(vacancyApplication);
        }
    }
    public class VacancyApplicationRemovedHandler : EventBaseHandler<VacancyApplicationRemoved>
    {
        public VacancyApplicationRemovedHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyApplicationRemoved msg)
        {
            _db.Delete<VacancyApplication>(msg.VacancyApplicationGuid);
        }
    }
    public class VacancyApplicationAppliedHandler : EventBaseHandler<VacancyApplicationApplied>
    {
        public VacancyApplicationAppliedHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyApplicationApplied msg)
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
    public class VacancyApplicationCancelledHandler : EventBaseHandler<VacancyApplicationCancelled>
    {
        public VacancyApplicationCancelledHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyApplicationCancelled msg)
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
    public class VacancyApplicationWonHandler : EventBaseHandler<VacancyApplicationWon>
    {
        public VacancyApplicationWonHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyApplicationWon msg)
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
    public class VacancyApplicationPretendedHandler : EventBaseHandler<VacancyApplicationPretended>
    {
        public VacancyApplicationPretendedHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyApplicationPretended msg)
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
    public class VacancyApplicationLostHandler : EventBaseHandler<VacancyApplicationLost>
    {
        public VacancyApplicationLostHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyApplicationLost msg)
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