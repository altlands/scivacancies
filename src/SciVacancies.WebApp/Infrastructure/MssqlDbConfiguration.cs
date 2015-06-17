using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.WebApp.Infrastructure
{
    public class MssqlDbConfiguration : DbConfiguration
    {
        public MssqlDbConfiguration()
        {           
            SetDefaultConnectionFactory(new SqlConnectionFactory());
        }
    }
}
