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
    public class PositionCreatedHandler : EventBaseHandler<PositionCreated>
    {
        public PositionCreatedHandler(IDatabase db) : base(db) { }
        public override void Handle(PositionCreated msg)
        {
            Position position = new Position()
            {
                Guid = msg.PositionGuid,
                OrganizationGuid = msg.OrganizationGuid,
                PositionTypeGuid=msg.Data.PositionTypeGuid,

                Name = msg.Data.Name,
                FullName = msg.Data.FullName,

                CreationdDate = msg.TimeStamp
            };

            _db.Insert(position);
        }
    }
    public class PositionUpdatedHandler : EventBaseHandler<PositionUpdated>
    {
        public PositionUpdatedHandler(IDatabase db) : base(db) { }
        public override void Handle(PositionUpdated msg)
        {
            Position position = _db.SingleById<Position>(msg.PositionGuid);
            
            //position.

            _db.Update(position);
        }
    }
    public class PositionRemovedHandler : EventBaseHandler<PositionRemoved>
    {
        public PositionRemovedHandler(IDatabase db) : base(db) { }
        public override void Handle(PositionRemoved msg)
        {
            _db.Delete<Position>(msg.PositionGuid);
        }
    }
}