using System;
using System.Data;
using Autofac;
using CommonDomain;
using CommonDomain.Core;
using CommonDomain.Persistence;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql;
using NEventStore.Persistence.Sql.SqlDialects;
using Npgsql;
using NPoco;
using SciVacancies.WebApp.Infrastructure.Saga;
using IConstructSagas = SciVacancies.WebApp.Infrastructure.Saga.IConstructSagas;
using ISagaRepository = SciVacancies.WebApp.Infrastructure.Saga.ISagaRepository;

namespace SciVacancies.WebApp.Infrastructure
{
    public class EventStoreModule : Module
    {
        private readonly byte[] encryptionKey = new byte[] { 0x45, 0x1a, 0x5, 0xf3, 0x4b, 0x55, 0x21, 0xaf, 0x22, 0x9f, 0x11, 0x2, 0x4, 0x4, 0xde, 0x0 };

        public IConfiguration Config { get; set; }
        private readonly ILogger _logger;

        public EventStoreModule(IConfiguration cfg, ILoggerFactory loggerFactory)
        {
            Config = cfg;
            _logger = loggerFactory.CreateLogger<EventBusModule>();
            _logger.LogDebug("Constructing EventStoreModule");
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new AggregateFactory())
                .As<IConstructAggregates>()
                .SingleInstance();
            builder.Register(c => new ConflictDetector())
                .As<IDetectConflicts>()
                .SingleInstance();

            builder.Register(c => GetEventStore(c.Resolve<IMediator>()))
                .As<IStoreEvents>()
                .SingleInstance();

            builder.Register(c => new EventStoreRepository(c.Resolve<IStoreEvents>(), c.Resolve<IConstructAggregates>(), c.Resolve<IDetectConflicts>()))
                    .As<IRepository>()
                    .SingleInstance();

            //builder.Register(c => new EventStoreRepository(c.Resolve<IStoreEvents>(), c.Resolve<IConstructAggregates>(), c.Resolve<IDetectConflicts>()))
            //    .As<IRepository>()
            //    .SingleInstance();

            //sagas start
            builder.Register(c => new SagaFactory())
                .As<IConstructSagas>()
                .SingleInstance();
            builder.Register(c => new SagaEventStoreRepository(c.Resolve<IStoreEvents>(), c.Resolve<IConstructSagas>()))
                .As<ISagaRepository>()
                .SingleInstance();
            //sagas end
        }
        private IStoreEvents GetEventStore(IMediator mediator)
        {
            return Wireup.Init()
                .UsingSqlPersistence(new NpgsqlConnectionFactory(Config["Data:EventStoreDb"]))
                .WithDialect(new PostgreSqlDialect())
                .EnlistInAmbientTransaction()
                .InitializeStorageEngine()
                .UsingJsonSerialization()
                .Compress()
                //.EncryptWith(encryptionKey)
                .UsingSynchronousDispatchScheduler()
                .DispatchTo(new DelegateMessageDispatcher(commit =>
                {
                    try
                    {
                        foreach (var e in commit.Events)
                        {
                            _logger.LogInformation("Dispatching event " + e.Body.ToString());
                            mediator.Publish(e.Body as dynamic);
                        }
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError("Error during dispatching commit", exception);
                        try
                        {
                            using (var db = new Database(Config["Data:ReadModelDb"], NpgsqlFactory.Instance))
                            {
                                using (var transaction = db.GetTransaction())
                                {
                                    db.Execute("INSERT INTO undispatched_commits SELECT * FROM commits WHERE commitid = @0", commit.CommitId);
                                    db.Execute("DELETE FROM commits WHERE commitid = @0", commit.CommitId);

                                    transaction.Complete();
                                }
                            }
                        }
                        catch (Exception dbException)
                        {
                            _logger.LogError("Error during clearing undispatched commit in eventstore", dbException);
                        }
                    }
                }))
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
