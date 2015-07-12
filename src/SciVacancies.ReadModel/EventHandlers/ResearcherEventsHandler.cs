using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using MediatR;
using NPoco;
using AutoMapper;

namespace SciVacancies.ReadModel.EventHandlers
{
    public class ResearcherEventsHandler :
        INotificationHandler<ResearcherCreated>,
        INotificationHandler<ResearcherUpdated>,
        INotificationHandler<ResearcherRemoved>
    {
        private readonly IDatabase _db;

        public ResearcherEventsHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(ResearcherCreated msg)
        {
            Researcher researcher = Mapper.Map<Researcher>(msg);

            using (var transaction = _db.GetTransaction())
            {
                _db.Insert(researcher);
                transaction.Complete();
            }
        }
        public void Handle(ResearcherUpdated msg)
        {
            Researcher researcher = _db.SingleById<Researcher>(msg.ResearcherGuid);

            using (var transaction = _db.GetTransaction())
            {

                _db.Update(researcher);
                transaction.Complete();
            }
        }
        public void Handle(ResearcherRemoved msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE res_researchers SET status = @0, update_date = @1 WHERE guid = @2", ResearcherStatus.Removed, msg.TimeStamp, msg.ResearcherGuid));
                transaction.Complete();
            }
        }
    }
}
