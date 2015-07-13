using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.ElasticSearchModel.Model;

using MediatR;
using Nest;
using AutoMapper;

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

        public void Handle(OrganizationCreated msg)
        {
            Organization organization = Mapper.Map<Organization>(msg);

            _elasticClient.Index(organization);
        }
        public void Handle(OrganizationUpdated msg)
        {
            Organization organization = Mapper.Map<Organization>(msg);

            _elasticClient.Update<Organization>(u => u.IdFrom(organization).Doc(organization));
        }
        public void Handle(OrganizationRemoved msg)
        {
            _elasticClient.Delete<Organization>(msg.OrganizationGuid.ToString());
        }
    }
}
