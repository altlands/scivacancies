using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;

using Autofac;
using CommonDomain;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql;
using NEventStore.Persistence.Sql.SqlDialects;
using MediatR;
using Npgsql;

namespace SciVacancies.WebApp.Infrastructure
{
    public class EventStoreModule : Module
    {
        private readonly byte[] encryptionKey = new byte[] { 0x45, 0x1a, 0x5, 0xf3, 0x4b, 0x55, 0x21, 0xaf, 0x22, 0x9f, 0x11, 0x2, 0x4, 0x4, 0xde, 0x0 };

        public IConfiguration Config { get; set; }

        public EventStoreModule(IConfiguration cfg)
        {
            Config = cfg;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new AggregateFactory()).As<IConstructAggregates>().SingleInstance();
            builder.Register(c => new ConflictDetector()).As<IDetectConflicts>().SingleInstance();

            //builder.Register(c => GetStubEventStore()).As<IStoreEvents>().SingleInstance();
            builder.Register(c => GetEventStore(c.Resolve<IMediator>())).As<IStoreEvents>().SingleInstance();
            //builder.Register(c => GetEventStoreWithoutDispatchers()).As<IStoreEvents>().SingleInstance();

            builder.Register(c => new EventStoreRepository(c.Resolve<IStoreEvents>(), c.Resolve<IConstructAggregates>(), c.Resolve<IDetectConflicts>())).As<IRepository>().SingleInstance();
        }
        private IStoreEvents GetEventStore(IMediator mediator)
        {
            return Wireup.Init()
                .UsingSqlPersistence(new NpgsqlConnectionFactory(Config.Get("Data:EventStoreDb:ConnectionString")))
                .WithDialect(new PostgreSqlDialect())
                .EnlistInAmbientTransaction()
                .InitializeStorageEngine()
                .UsingJsonSerialization()
                .Compress()
                //.EncryptWith(encryptionKey)
                .UsingSynchronousDispatchScheduler()
                .DispatchTo(new DelegateMessageDispatcher(commit =>
                {
                    foreach (var e in commit.Events)
                    {
                        mediator.Publish(e.Body as dynamic);
                    }
                }))
                .Build();
        }
        private IStoreEvents GetEventStoreWithoutDispatchers()
        {
            return Wireup.Init()
                .UsingSqlPersistence(new NpgsqlConnectionFactory(Config.Get("Data:EventStoreDb:ConnectionString")))
                .WithDialect(new PostgreSqlDialect())
                .EnlistInAmbientTransaction()
                .InitializeStorageEngine()
                .UsingJsonSerialization()
                .Compress()
                //.EncryptWith(encryptionKey)
                .Build();
        }
        private IStoreEvents GetStubEventStore()
        {
            return Wireup.Init()
                .UsingInMemoryPersistence()
                .EnlistInAmbientTransaction()
                .InitializeStorageEngine()
                .UsingJsonSerialization()
                .Compress()
                .Build();
        }
    }
    public class NpgsqlConnectionFactory : IConnectionFactory
    {
        private string _connectionString;
        public NpgsqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public Type GetDbProviderFactoryType()
        {
            return typeof(NpgsqlFactory);
        }

        public IDbConnection Open()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
