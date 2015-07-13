using System;
using System.Data;
using System.Data.SqlClient;
using CommonDomain;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;
using Microsoft.Framework.ConfigurationModel;
using NEventStore;
using NEventStore.Persistence.Sql;
using NEventStore.Persistence.Sql.SqlDialects;
using SciVacancies.Domain.Aggregates;
using SciVacancies.Domain.DataModels;
using Xunit;

namespace SciVacancies.IntegrationTests
{
    public class NullAggregateFactory : IConstructAggregates
    {
        public IAggregate Build(Type type, Guid id, IMemento snapshot)
        {
            return (IAggregate)Activator.CreateInstance(type);
        }
    }

    public class MssqlConnectionFactory : IConnectionFactory
    {
        private string _connectionString;
        public MssqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public Type GetDbProviderFactoryType()
        {
            return typeof(SqlClientFactory);
        }

        public IDbConnection Open()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }

    public class AggregateTests
    {
        public IConfiguration Config { get; set; }

        public AggregateTests()
        {
            var config = new Configuration();
            config.AddJsonFile("config.json");
            Config = config;
        }

        public IStoreEvents GetEventStore()
        {
            return Wireup.Init()
                    .UsingSqlPersistence(new MssqlConnectionFactory(Config.Get("DB:ConnectionString")))
                        .WithDialect(new MsSqlDialect())
                        .InitializeStorageEngine()
                        .UsingJsonSerialization()
                            .Compress()
                  .Build();
        }

        [Fact]
        public void Test1()
        {
            var store = GetEventStore();
            var repository = new EventStoreRepository(store, new NullAggregateFactory(), new ConflictDetector());
            var id = Guid.NewGuid();
            var org1 = new Organization(id, new OrganizationDataModel() { Name = "Российский Фонд Фундаментальных Исследований", ShortName="РФФИ" });            
            repository.Save(org1, Guid.NewGuid(), null);
            var org2 = repository.GetById<Organization>(id);

            Assert.NotNull(org2);
            Assert.Equal(id, org2.Id);
            //Assert.Equal("Российский Фонд Фундаментальных Исследований", org2.Data.Name);
            //Assert.Equal("РФФИ", org2.);
        }
        [Fact]
        public void OrganizationRegistration()
        {

        }
        [Fact]
        public void ResearcherRegistration()
        {

        }
    }
}
