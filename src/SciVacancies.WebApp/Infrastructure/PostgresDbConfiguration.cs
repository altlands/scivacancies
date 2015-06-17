using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;

namespace SciVacancies.WebApp.Infrastructure
{
    public class PostgresDbConfiguration : DbConfiguration
    {
        public PostgresDbConfiguration()
        {
            SetProviderServices("Npgsql", new Npgsql.NpgsqlServices());
            SetMigrationSqlGenerator("Npgsql", () => new Npgsql.NpgsqlMigrationSqlGenerator());
            SetDefaultConnectionFactory(new Npgsql.NpgsqlConnectionFactory());
            SetProviderFactory("Npgsql", Npgsql.NpgsqlFactory.Instance);
        }
    }
}
