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
        public void Handle(VacancyApplicationCreated message)
        {
            var vacancyApplication = Mapper.Map<VacancyApplication>(message);

            using (var transaction = _db.GetTransaction())
            {
                _db.Insert(vacancyApplication);
                if (vacancyApplication.attachments != null)
                {
                    foreach (var at in vacancyApplication.attachments)
                    {
                        if (at.guid == Guid.Empty) at.guid = Guid.NewGuid();
                        at.vacancyapplication_guid = vacancyApplication.guid;
                        _db.Insert(at);
                    }
                }

                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationUpdated message)
        {
            var vacancyApplication = _db.SingleById<VacancyApplication>(message.VacancyApplicationGuid);

            var updatedVacancyApplication = Mapper.Map<VacancyApplication>(message);
            updatedVacancyApplication.creation_date = vacancyApplication.creation_date;

            using (var transaction = _db.GetTransaction())
            {
                _db.Update(updatedVacancyApplication);
                //TODO - без удаления
                _db.Execute(new Sql($"DELETE FROM res_vacancyapplication_attachments WHERE vacancyapplication_guid = @0", message.VacancyApplicationGuid));
                if (updatedVacancyApplication.attachments != null)
                {
                    foreach (var at in updatedVacancyApplication.attachments)
                    {
                        if (at.guid == Guid.Empty) at.guid = Guid.NewGuid();
                        at.vacancyapplication_guid = vacancyApplication.guid;
                        _db.Insert(at);
                    }
                }

                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationRemoved message)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE res_vacancyapplications SET status = @0, update_date = @1 WHERE guid = @2", VacancyApplicationStatus.Removed, message.TimeStamp, message.VacancyApplicationGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationApplied message)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE res_vacancyapplications SET apply_date = @0, status = @1, update_date = @2 WHERE guid = @3", message.TimeStamp, VacancyApplicationStatus.Applied, message.TimeStamp, message.VacancyApplicationGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationCancelled message)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE res_vacancyapplications SET status = @0, update_date = @1 WHERE guid = @2", VacancyApplicationStatus.Cancelled, message.TimeStamp, message.VacancyApplicationGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationWon message)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE res_vacancyapplications SET status = @0, update_date = @1 WHERE guid = @2", VacancyApplicationStatus.Won, message.TimeStamp, message.VacancyApplicationGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationPretended message)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE res_vacancyapplications SET status = @0, update_date = @1 WHERE guid = @2", VacancyApplicationStatus.Pretended, message.TimeStamp, message.VacancyApplicationGuid));
                transaction.Complete();
            }
        }
        public void Handle(VacancyApplicationLost message)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE res_vacancyapplications SET status = @0, update_date = @1 WHERE guid = @2", VacancyApplicationStatus.Lost, message.TimeStamp, message.VacancyApplicationGuid));
                transaction.Complete();
            }
        }
    }
}