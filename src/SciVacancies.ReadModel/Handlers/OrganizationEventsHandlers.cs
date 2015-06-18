﻿using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Handlers
{
    public class OrganizationCreatedHandler : EventBaseHandler<OrganizationCreated>
    {
        public OrganizationCreatedHandler(IDatabase db) : base(db) { }
        public override void Handle(OrganizationCreated msg)
        {
            //TODO
        }
    }
    public class OrganizationUpdatedHandler : EventBaseHandler<OrganizationUpdated>
    {
        public OrganizationUpdatedHandler(IDatabase db) : base(db) { }
        public override void Handle(OrganizationUpdated msg)
        {
            //TODO
        }
    }
    public class OrganizationRemovedHandler : EventBaseHandler<OrganizationRemoved>
    {
        public OrganizationRemovedHandler(IDatabase db) : base(db) { }
        public override void Handle(OrganizationRemoved msg)
        {
            _db.Delete<Organization>(msg.OrganizationGuid);
        }
    }
}