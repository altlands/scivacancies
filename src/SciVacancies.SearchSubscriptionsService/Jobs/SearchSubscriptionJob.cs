using System;
using Microsoft.Framework.Configuration;
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
using SciVacancies.Services.Elastic;
using Nest;

namespace SciVacancies.SearchSubscriptionsService.Jobs
{
    public class SearchSubscriptionJob : IJob
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ILogger _logger;

        public SearchSubscriptionJob(ILifetimeScope lifetimeScope, ILoggerFactory loggerFactory)
        {
            _lifetimeScope = lifetimeScope;
            _logger = loggerFactory.CreateLogger<SearchSubscriptionJob>();
        }

        /// <summary>
        /// выполнить работу
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            ExecuteJob();
        }

        /// <summary>
        /// выполнить работу
        /// </summary>
        public void ExecuteJob()
        {
            _logger.LogInformation("SearchSubscriptionJob started");
            try
            {
                var dataBase = _lifetimeScope.Resolve<IDatabase>();

                _logger.LogInformation($"SearchSubscriptionJob: Select Subscriptions from DB");
                Queue<ReadModel.Core.SearchSubscription> subscriptionQueue = new Queue<ReadModel.Core.SearchSubscription>(dataBase.Fetch<ReadModel.Core.SearchSubscription>(new Sql($"SELECT * FROM res_searchsubscriptions ss WHERE ss.status = @0", SearchSubscriptionStatus.Active)));
                _logger.LogInformation($"SearchSubscriptionJob: Found {subscriptionQueue.Count} Subscriptions in DB");

                //int poolCount = 20;
                //var threadSize = subscriptionQueue.Count > poolCount
                //    ? (subscriptionQueue.Count / poolCount) + 1
                //    : 1;

                //_logger.LogInformation($"SearchSubscriptionJob: threadSize = {threadSize}");
                //var i = 0;

                //var actions = new List<Action>();

                //while (subscriptionQueue.Skip(threadSize * i).Take(threadSize).Any())
                //{
                //    var y = i;
                //    var scanner = _lifetimeScope.Resolve<ISearchSubscriptionScanner>();
                //    actions.Add(() =>
                //    {
                //        scanner.Initialize(subscriptionQueue.Skip(threadSize * y).Take(threadSize));
                //        scanner.PoolHandleSubscriptions();
                //    });
                //    i++;
                //}
                //var actionsArray = actions.ToArray();

                _logger.LogInformation("SearchSubscriptionJob: Обработка потоков началась");

                //Parallel.Invoke(actionsArray);
                var scanner = _lifetimeScope.Resolve<ISearchSubscriptionScanner>();

                _logger.LogInformation("SearchSubscriptionJob: Scanner initializaing");
                scanner.Initialize(subscriptionQueue);
                _logger.LogInformation("SearchSubscriptionJob: Scanner initialized");
                _logger.LogInformation("SearchSubscriptionJob: Scanner subsciptions starting to invoke");
                scanner.PoolHandleSubscriptions();
                _logger.LogInformation("SearchSubscriptionJob: Scanner subsciptions finished to invoke");

                _logger.LogInformation("SearchSubscriptionJob: Обработка потоков завершена");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }
    }
}