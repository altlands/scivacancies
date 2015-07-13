using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;

using MediatR;
using NPoco;
using AutoMapper;

namespace SciVacancies.ReadModel.EventHandlers
{
    public class VacancyApplicationEventsHandler :
        INotificationHandler<VacancyApplicationCreated>,
        INotificationHandler<VacancyApplicationUpdated>,
        INotificationHandler<VacancyApplicationRemoved>,
        INotificationHandler<VacancyApplicationApplied>,
        INotificationHandler<VacancyApplicationCancelled>,
        INotificationHandler<VacancyApplicationWon>,
        INotificationHandler<VacancyApplicationPretended>,
        INotificationHandler<VacancyApplicationLost>
    {
        private readonly IDatabase _db;

        public VacancyApplicationEventsHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(VacancyApplicationCreated msg)
        {
            VacancyApplication vacancyApplication = Mapper.Map<VacancyApplication>(msg);

            using (var transaction = _db.GetTransaction())
            {

                _db.Insert(vacancyApplication);
                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationUpdated msg)
        {
            VacancyApplication vacancyApplication = _db.SingleById<VacancyApplication>(msg.VacancyApplicationGuid);

            VacancyApplication updatedVacancyApplication = Mapper.Map<VacancyApplication>(msg);

            using (var transaction = _db.GetTransaction())
            {

                _db.Update(vacancyApplication);
                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationRemoved msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE res_vacancyapplications SET status = @0, update_date = @1 WHERE guid = @2", VacancyApplicationStatus.Removed, msg.TimeStamp, msg.VacancyApplicationGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationApplied msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE res_vacancyapplications SET status = @0, update_date = @1 WHERE guid = @2", VacancyApplicationStatus.Applied, msg.TimeStamp, msg.VacancyApplicationGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationCancelled msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE res_vacancyapplications SET status = @0, update_date = @1 WHERE guid = @2", VacancyApplicationStatus.Cancelled, msg.TimeStamp, msg.VacancyApplicationGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationWon msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE res_vacancyapplications SET status = @0, update_date = @1 WHERE guid = @2", VacancyApplicationStatus.Won, msg.TimeStamp, msg.VacancyApplicationGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationPretended msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE res_vacancyapplications SET status = @0, update_date = @1 WHERE guid = @2", VacancyApplicationStatus.Pretended, msg.TimeStamp, msg.VacancyApplicationGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationLost msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE res_vacancyapplications SET status = @0, update_date = @1 WHERE guid = @2", VacancyApplicationStatus.Lost, msg.TimeStamp, msg.VacancyApplicationGuid));
                transaction.Complete();
            }
        }
    }
}