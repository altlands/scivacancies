using MediatR;
using Nest;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.ElasticSearchModel.Model;

namespace SciVacancies.ReadModel.ElasticSearchModel.EventHandlers
{
    public class OrganizationEventsHandler :
        INotificationHandler<OrganizationCreated>,
        INotificationHandler<OrganizationUpdated>,
        INotificationHandler<OrganizationRemoved>

    {
        private readonly IElasticClient _elasticClient;

        public OrganizationEventsHandler(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public void Handle(OrganizationCreated notification)
        {
            Organization organization = new Organization()
            {
                Id = notification.OrganizationGuid,

                Name = notification.Data.Name,
                NameEng = notification.Data.NameEng,
                ShortName = notification.Data.ShortName,
                ShortNameEng = notification.Data.ShortNameEng,

                CityName = notification.Data.CityName,
                Address = notification.Data.Address,
                Website = notification.Data.Website,
                Email = notification.Data.Email,

                INN = notification.Data.INN,
                OGRN = notification.Data.OGRN,

                OrgForm = notification.Data.OrgForm,
                OrgFormId = notification.Data.OrgFormId,

                PublishedVacancies = notification.Data.PublishedVacancies,

                Foiv = notification.Data.Foiv,
                FoivId = notification.Data.FoivId,

                Activity = notification.Data.Activity,
                ActivityId = notification.Data.ActivityId,

                HeadFirstName = notification.Data.HeadFirstName,
                HeadLastName = notification.Data.HeadLastName,
                HeadPatronymic = notification.Data.HeadPatronymic,

                CreationDate = notification.TimeStamp
            };

            _elasticClient.Index(organization);
        }

        public void Handle(OrganizationUpdated notification)
        {
            Organization organization = _elasticClient.Get<Organization>(notification.OrganizationGuid.ToString()).Source;

            organization.Name = notification.Data.Name;
            organization.NameEng = notification.Data.NameEng;
            organization.ShortName = notification.Data.ShortName;
            organization.ShortNameEng = notification.Data.ShortNameEng;

            organization.CityName = notification.Data.CityName;
            organization.Address = notification.Data.Address;
            organization.Website = notification.Data.Website;
            organization.Email = notification.Data.Email;

            organization.INN = notification.Data.INN;
            organization.OGRN = notification.Data.OGRN;

            organization.OrgForm = notification.Data.OrgForm;
            organization.OrgFormId = notification.Data.OrgFormId;

            organization.PublishedVacancies = notification.Data.PublishedVacancies;

            organization.Foiv = notification.Data.Foiv;
            organization.FoivId = notification.Data.FoivId;

            organization.Activity = notification.Data.Activity;
            organization.ActivityId = notification.Data.ActivityId;

            organization.HeadFirstName = notification.Data.HeadFirstName;
            organization.HeadLastName = notification.Data.HeadLastName;
            organization.HeadPatronymic = notification.Data.HeadPatronymic;

            organization.UpdateDate = notification.TimeStamp;

            _elasticClient.Update<Organization>(u => u.IdFrom(organization).Doc(organization));
        }

        public void Handle(OrganizationRemoved notification)
        {
            _elasticClient.Delete<Organization>(notification.OrganizationGuid.ToString());
        }
    }
}
