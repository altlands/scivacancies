using MediatR;
using Nest;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.ElasticSearchModel.Model;

namespace SciVacancies.ReadModel.ElasticSearchModel.EventHandlers
{    
    public class OrganizationCreatedHandler : INotificationHandler<OrganizationCreated>
    {
        private readonly IElasticClient _elasticClient;

        public OrganizationCreatedHandler(IElasticClient elasticClient)
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
                OrgFormGuid = notification.Data.OrgFormGuid,

                PublishedVacancies = notification.Data.PublishedVacancies,

                Foiv = notification.Data.Foiv,
                FoivId = notification.Data.FoivId,
                FoivGuid = notification.Data.FoivGuid,

                Activity = notification.Data.Activity,
                ActivityId = notification.Data.ActivityId,
                ActivityGuid = notification.Data.ActivityGuid,                

                HeadFirstName = notification.Data.HeadFirstName,
                HeadLastName = notification.Data.HeadLastName,
                HeadPatronymic = notification.Data.HeadPatronymic
            };

            _elasticClient.Index(organization);
        }
    }
}
