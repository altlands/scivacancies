﻿using SciVacancies.Domain.Events;
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
            //TODO
        }
    }
    public class PositionUpdatedHandler : EventBaseHandler<PositionUpdated>
    {
        public PositionUpdatedHandler(IDatabase db) : base(db) { }
        public override void Handle(PositionUpdated msg)
        {
            //TODO
        }
    }
    public class PositionRemovedHandler : EventBaseHandler<PositionRemoved>
    {
        public PositionRemovedHandler(IDatabase db) : base(db) { }
        public override void Handle(PositionRemoved msg)
        {
            //TODO
        }
    }
}