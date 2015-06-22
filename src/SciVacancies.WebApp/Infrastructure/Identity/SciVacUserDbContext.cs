using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SciVacancies.WebApp.Infrastructure.Identity
{
    [DbConfigurationType(typeof(MssqlDbConfiguration))]
    public class SciVacUserDbContext : IdentityDbContext<SciVacUser>
    {
        public SciVacUserDbContext(string connectionString) : base(connectionString, throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
    }
}
