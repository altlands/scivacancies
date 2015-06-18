using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.Domain.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CommonDomain.Persistence;

namespace SciVacancies.Domain.Aggregates.Services
{
    public class OrganizationService : IOrganizationService
    {
        private IRepository _repository;

        public OrganizationService(IRepository repository)
        {
            _repository = repository;
        }

        public Guid CreateOrganization(OrganizationDataModel data)
        {
            Organization organization = new Organization(Guid.NewGuid(), data);
            _repository.Save(organization, Guid.NewGuid(), null);

            return organization.Id;
        }
        public void UpdateOrganization(Guid organizationGuid, OrganizationDataModel data)
        {
            Organization organization = _repository.GetById<Organization>(organizationGuid);
            organization.Update(data);
            _repository.Save(organization, Guid.NewGuid(), null);
        }
        public void RemoveOrganization(Guid organizationGuid)
        {
            Organization organization = _repository.GetById<Organization>(organizationGuid);
            organization.Remove();
            _repository.Save(organization, Guid.NewGuid(), null);
        }

        public Guid CreatePosition(Guid organizationGuid, PositionDataModel data)
        {
            Organization organization = _repository.GetById<Organization>(organizationGuid);
            Guid positionGuid = organization.CreatePosition(data);

            return positionGuid;
        }
        public void UpdatePosition(Guid organizationGuid, Guid positionGuid, PositionDataModel data)
        {
            Organization organization = _repository.GetById<Organization>(organizationGuid);
            organization.UpdatePosition(positionGuid, data);
        }
        public void RemovePosition(Guid organizationGuid, Guid positionGuid)
        {
            Organization organization = _repository.GetById<Organization>(organizationGuid);
            organization.RemovePosition(positionGuid);
        }

        public Guid PublishVacancy(Guid organizationGuid, Guid positionGuid, VacancyDataModel data)
        {
            Organization organization = _repository.GetById<Organization>(organizationGuid);
            Guid vacancyGuid = organization.PublishVacancy(positionGuid, data);

            return vacancyGuid;
        }
        public void SwitchVacancyToAcceptApplications(Guid organizationGuid, Guid vacancyGuid)
        {
            Organization organization = _repository.GetById<Organization>(organizationGuid);
            organization.SwitchVacancyToAcceptApplications(vacancyGuid);
        }
        public void SwitchVacancyInCommittee(Guid organizationGuid, Guid vacancyGuid)
        {
            Organization organization = _repository.GetById<Organization>(organizationGuid);
            organization.SwitchVacancyInCommittee(vacancyGuid);
        }
        public void CloseVacancy(Guid organizationGuid, Guid vacancyGuid, Guid winnerGuid, Guid pretenderGuid)
        {
            Organization organization = _repository.GetById<Organization>(organizationGuid);
            organization.CloseVacancy(vacancyGuid, winnerGuid, pretenderGuid);
        }
        public void CancelVacancy(Guid organizationGuid, Guid vacancyGuid, string reason)
        {
            Organization organization = _repository.GetById<Organization>(organizationGuid);
            organization.CancelVacancy(vacancyGuid, reason);
        }
    }
}
