using System;
using System.Data;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.ReadModel
{
    public interface IOrganizationRepository : IDbConnection, IDbTransaction
    {
        Organization GetOrganizationById(Guid id);
    }
}
