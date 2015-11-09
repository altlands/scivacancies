using System;
using Quartz;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Npgsql;
using NPoco;
using SciVacancies.Domain.Enums;
using Microsoft.Framework.Logging;

namespace SciVacancies.SearchSubscriptionsService.Jobs
{
    public class SearchSubscriptionJob : IJob
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ILogger Logger;

        public SearchSubscriptionJob(ILifetimeScope lifetimeScope, ILoggerFactory loggerFactory)
        {
            _lifetimeScope = lifetimeScope;
            this.Logger = loggerFactory.CreateLogger<SearchSubscriptionJob>();
        }

        /// <summary>
        /// выполнить работу
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Logger.LogInformation("SearchSubscriptionJob started");
            try
            {
                var dataBase = _lifetimeScope.Resolve<IDatabase>();

                //TODO - загружать по частям, вызывая хранимую процедуру
                Queue<SciVacancies.ReadModel.Core.SearchSubscription> subscriptionQueue = new Queue<ReadModel.Core.SearchSubscription>(dataBase.Fetch<SciVacancies.ReadModel.Core.SearchSubscription>(new Sql($"SELECT * FROM res_searchsubscriptions ss WHERE ss.status = @0", SearchSubscriptionStatus.Active)));

                int poolCount = 20;
                var threadSize = subscriptionQueue.Count > poolCount
                    ? (subscriptionQueue.Count / poolCount) + 1
                    : 1;

                Logger.LogInformation($"SearchSubscriptionJob: threadSize = {threadSize}");
                var i = 0;

                var actions = new List<Action>();

                Logger.LogInformation($"SearchSubscriptionJob: Нашли {subscriptionQueue.Count} подписок");

                while (subscriptionQueue.Skip(threadSize * i).Take(threadSize).Any())
                {
                    var y = i;
                    var scanner = _lifetimeScope.Resolve<ISearchSubscriptionScanner>();
                    actions.Add(() =>
                    {
                        scanner.Initialize(subscriptionQueue.Skip(threadSize * y).Take(threadSize));
                        scanner.PoolHandleSubscriptions();
                    });
                    i++;
                }
                var actionsArray = actions.ToArray();

                Logger.LogInformation("SearchSubscriptionJob: Обработка потоков началась");

                Parallel.Invoke(actionsArray);

                Logger.LogInformation("SearchSubscriptionJob: Обработка потоков завершена");
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
            }
        }
    }
}