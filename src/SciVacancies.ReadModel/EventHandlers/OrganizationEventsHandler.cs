using MediatR;
using NPoco;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.ReadModel.EventHandlers
{
    public class OrganizationEventsHandler : 
        INotificationHandler<OrganizationCreated>,
        INotificationHandler<OrganizationUpdated>,
        INotificationHandler<OrganizationRemoved>
    {
        private readonly IDatabase _db;

        public OrganizationEventsHandler(IDatabase db)
        {
            _db = db;
        }

        public void Handle(OrganizationCreated msg)
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
        }
        public void Handle(OrganizationUpdated msg)
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
        }
        public void Handle(OrganizationRemoved msg)
        {
            //TODO: Should we remove all the related entities? Vacancies, etc
            //TODO: SHould we delete or mark as "Deleted"? - Нужно сделать по два метода на каждую сущность: удалить с каскадом, пометить как "удалено".
            _db.Delete<Organization>(msg.OrganizationGuid);
        }
    }    
}