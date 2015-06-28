using MediatR;
using Nest;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.ElasticSearchModel.Model;

namespace SciVacancies.ReadModel.ElasticSearchModel.EventHandlers
{    
    public class OrganizationRemovedHandler : INotificationHandler<OrganizationRemoved>
    {
        private readonly IElasticClient _elasticClient;

        public OrganizationRemovedHandler(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public void Handle(OrganizationRemoved notification)
        {
            _elasticClient.Delete<Organization>(notification.OrganizationGuid.ToString());
        }
    }
}
