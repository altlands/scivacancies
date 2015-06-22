using System.Data.Entity;
using Npgsql;

namespace SciVacancies.WebApp.Infrastructure.Identity
{
    public class PostgresDbConfiguration : DbConfiguration
    {
        public PostgresDbConfiguration()
        {
            SetProviderServices("Npgsql", new NpgsqlServices());
            SetMigrationSqlGenerator("Npgsql", () => new NpgsqlMigrationSqlGenerator());
            SetDefaultConnectionFactory(new Npgsql.NpgsqlConnectionFactory());
            SetProviderFactory("Npgsql", NpgsqlFactory.Instance);
        }
    }
}
