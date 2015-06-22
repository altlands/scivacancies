using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SciVacancies.WebApp.Infrastructure.Identity
{
    public class MssqlDbConfiguration : DbConfiguration
    {
        public MssqlDbConfiguration()
        {           
            SetDefaultConnectionFactory(new SqlConnectionFactory());
        }
    }
}
