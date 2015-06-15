using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Interfaces
{
    public interface IOrganizationService
    {
        Guid CreateOrganization(OrganizationDataModel data);
        void UpdateOrganization(Guid organizationGuid, OrganizationDataModel data);
        void RemoveOrganization(Guid organizationGuid);

        Guid CreatePosition(Guid organizationGuid, PositionDataModel data);
        void UpdatePosition(Guid organizationGuid, Guid positionGuid, PositionDataModel data);
        void RemovePosition(Guid organizationGuid, Guid positionGuid);

        Guid PublishVacancy(Guid organizationGuid, Guid positionGuid, VacancyDataModel data);
        void SwitchVacancyToAcceptApplications(Guid organizationGuid, Guid vacancyGuid);
        void SwitchVacancyInCommittee(Guid organizationGuid, Guid vacancyGuid);
        void CloseVacancy(Guid organizationGuid, Guid vacancyGuid, Guid winnerGuid, Guid pretenderGuid);
        void CancelVacancy(Guid organizationGuid, Guid vacancyGuid, string reason);
    }
}
