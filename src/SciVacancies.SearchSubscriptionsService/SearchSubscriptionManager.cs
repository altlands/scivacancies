using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Autofac;
using Npgsql;
using NPoco;
using SciVacancies.Domain.Enums;

namespace SciVacancies.SearchSubscriptionsService
{
    public class SearchSubscriptionManager : ISearchSubscriptionManager
    {
        private readonly ILifetimeScope _lifetimeScope;

        public SearchSubscriptionManager(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public void Combine()
        {
            var dataBase = new Database("Server = localhost; Database = scivacancies; User Id = postgres; Password = postgres", NpgsqlFactory.Instance);


            //TODO - загружать по частям, вызывая хранимую процедуру
            Queue<SciVacancies.ReadModel.Core.SearchSubscription> subscriptionQueue = new Queue<ReadModel.Core.SearchSubscription>(dataBase.Fetch<SciVacancies.ReadModel.Core.SearchSubscription>(new Sql($"SELECT * FROM res_searchsubscriptions ss WHERE ss.status = @0", SearchSubscriptionStatus.Active)));

            const int poolCount = 20;
            var threadSize = subscriptionQueue.Count > poolCount
                ? (subscriptionQueue.Count / poolCount) + 1
                : 1;
            var manualResetEventsArray = new ManualResetEvent[poolCount];
            var searchSubscriptionScannerArray = new ISearchSubscriptionScanner[poolCount];

            var i = 0;
            while (subscriptionQueue.Skip(threadSize * i).Take(threadSize).Any())
            {
                manualResetEventsArray[i] = new ManualResetEvent(false);
                searchSubscriptionScannerArray[i] = _lifetimeScope.Resolve<ISearchSubscriptionScanner>();
                searchSubscriptionScannerArray[i].Initialize(manualResetEventsArray[i], subscriptionQueue.Skip(threadSize * i).Take(threadSize));

                //todo ntemnikov parallel linq
                ThreadPool.QueueUserWorkItem(searchSubscriptionScannerArray[i].PoolHandleSubscriptions);
                manualResetEventsArray[i].Set();
                i++;
            }

        }
    }
}