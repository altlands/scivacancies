using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;
using Nest;

namespace SciVacancies.ReadModel.EventHandlers
{
    public class OrganizationCreatedHandler : EventBaseHandler<OrganizationCreated>
    {
        private readonly IElasticClient _elastic;

        public OrganizationCreatedHandler(IDatabase db, IElasticClient elastic) : base(db) { _elastic = elastic; }
        public override void Handle(OrganizationCreated msg)
        {
            Organization organization = new Organization()
            {
                Guid = msg.OrganizationGuid,

                Name = msg.Data.Name,
                NameEng = msg.Data.NameEng,
                ShortName = msg.Data.ShortName,
                ShortNameEng = msg.Data.ShortNameEng,

                CityName = msg.Data.CityName,
                Address = msg.Data.Address,
                Website = msg.Data.Website,
                Email = msg.Data.Email,

                INN = msg.Data.INN,
                OGRN = msg.Data.OGRN,

                OrgForm = msg.Data.OrgForm,
                OrgFormId = msg.Data.OrgFormId,
                OrgFormGuid = msg.Data.OrgFormGuid,

                PublishedVacancies = msg.Data.PublishedVacancies,

                Foiv = msg.Data.Foiv,
                FoivId = msg.Data.FoivId,
                FoivGuid = msg.Data.FoivGuid,

                Activity = msg.Data.Activity,
                ActivityId = msg.Data.ActivityId,
                ActivityGuid = msg.Data.ActivityGuid,

                ResearchDirections = msg.Data.ResearchDirections,

                HeadFirstName = msg.Data.HeadFirstName,
                HeadLastName = msg.Data.HeadLastName,
                HeadPatronymic = msg.Data.HeadPatronymic
            };

            _db.Insert(organization);

            _elastic.Index(organization);
            //_elastic.IndexOrganization(organization);
        }
    }
    public class OrganizationUpdatedHandler : EventBaseHandler<OrganizationUpdated>
    {
        private readonly IElasticService _elastic;

        public OrganizationUpdatedHandler(IDatabase db, IElasticService elastic) : base(db) { _elastic = elastic; }
        public override void Handle(OrganizationUpdated msg)
        {
            Organization organization = _db.SingleById<Organization>(msg.OrganizationGuid);

            organization.Name = msg.Data.Name;
            organization.NameEng = msg.Data.NameEng;
            organization.ShortName = msg.Data.ShortName;
            organization.ShortNameEng = msg.Data.ShortNameEng;

            organization.CityName = msg.Data.CityName;
            organization.Address = msg.Data.Address;
            organization.Website = msg.Data.Website;
            organization.Email = msg.Data.Email;

            organization.INN = msg.Data.INN;
            organization.OGRN = msg.Data.OGRN;

            organization.OrgForm = msg.Data.OrgForm;
            organization.OrgFormId = msg.Data.OrgFormId;
            organization.OrgFormGuid = msg.Data.OrgFormGuid;

            organization.PublishedVacancies = msg.Data.PublishedVacancies;

            organization.Foiv = msg.Data.Foiv;
            organization.FoivId = msg.Data.FoivId;
            organization.FoivGuid = msg.Data.FoivGuid;

            organization.Activity = msg.Data.Activity;
            organization.ActivityId = msg.Data.ActivityId;
            organization.ActivityGuid = msg.Data.ActivityGuid;

            organization.ResearchDirections = msg.Data.ResearchDirections;

            organization.HeadFirstName = msg.Data.HeadFirstName;
            organization.HeadLastName = msg.Data.HeadLastName;
            organization.HeadPatronymic = msg.Data.HeadPatronymic;

            _db.Update(organization);

            //_elastic
            _elastic.UpdateOrganization(organization);
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