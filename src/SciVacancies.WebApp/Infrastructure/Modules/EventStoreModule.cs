using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac;
using CommonDomain;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql.SqlDialects;

namespace SciVacancies.WebApp.Infrastructure
{
    public class EventStoreModule : Module
    {
        private readonly byte[] encryptionKey = new byte[] { 0x45, 0x1a, 0x5, 0xf3, 0x4b, 0x55, 0x21, 0xaf, 0x22, 0x9f, 0x11, 0x2, 0x4, 0x4, 0xde, 0x0 };
        private readonly string sqlConnectionStringName = "EventStore"; //TODO: move to a config file

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new AggregateFactory()).As<IConstructAggregates>().SingleInstance();
            builder.Register(c => new ConflictDetector()).As<IDetectConflicts>().SingleInstance();
            builder.Register(c => GetEventStore(c.Resolve<IDispatchCommits>())).As<IStoreEvents>().SingleInstance();
            builder.Register(c => new EventStoreRepository(c.Resolve<IStoreEvents>(), c.Resolve<IConstructAggregates>(), c.Resolve<IDetectConflicts>())).As<IRepository>().SingleInstance();
        }
        private IStoreEvents GetEventStore(IDispatchCommits bus)
        {
            return Wireup.Init()
                .UsingSqlPersistence(sqlConnectionStringName)
                .WithDialect(new PostgreSqlDialect())
                .EnlistInAmbientTransaction()
                .InitializeStorageEngine()
                .UsingJsonSerialization()
                .Compress()
                //.EncryptWith(encryptionKey)
                .UsingSynchronousDispatchScheduler()
                .DispatchTo(bus)
                .Build();
        }
    }
}
