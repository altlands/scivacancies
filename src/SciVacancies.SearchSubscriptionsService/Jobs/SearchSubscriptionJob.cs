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

namespace SciVacancies.SearchSubscriptionsService.Jobs
{
    public class SearchSubscriptionJob : IJob
    {
        //static readonly string ServiceId = "#1";
        //static readonly int Seed = 1;
        private readonly ILifetimeScope _lifetimeScope;

        public SearchSubscriptionJob(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// выполнить работу
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"SearchSubscriptionJob: Executing at UTC time: {DateTime.Now.ToUniversalTime().ToLongTimeString()}");

            try
            {
                var dataBase = _lifetimeScope.Resolve<IDatabase>();

                //TODO - загружать по частям, вызывая хранимую процедуру
                Queue<SciVacancies.ReadModel.Core.SearchSubscription> subscriptionQueue = new Queue<ReadModel.Core.SearchSubscription>(dataBase.Fetch<SciVacancies.ReadModel.Core.SearchSubscription>(new Sql($"SELECT * FROM res_searchsubscriptions ss WHERE ss.status = @0", SearchSubscriptionStatus.Active)));

                const int poolCount = 20;
                var threadSize = subscriptionQueue.Count > poolCount
                    ? (subscriptionQueue.Count / poolCount) + 1
                    : 1;

                Console.WriteLine($"threadSize = {threadSize}");
                //var manualResetEventsArray = new EventWaitHandle[poolCount];
                //var searchSubscriptionScannerArray = new ISearchSubscriptionScanner[poolCount];

                var i = 0;
                //while (subscriptionQueue.Skip(threadSize * i).Take(threadSize).Any())
                //{
                //    manualResetEventsArray[i] = new EventWaitHandle(false, EventResetMode.AutoReset);
                //    searchSubscriptionScannerArray[i] = _lifetimeScope.Resolve<ISearchSubscriptionScanner>();
                //    searchSubscriptionScannerArray[i].Initialize(manualResetEventsArray[i], subscriptionQueue.Skip(threadSize * i).Take(threadSize));

                //    //todo ntemnikov parallel linq
                //    ThreadPool.QueueUserWorkItem(searchSubscriptionScannerArray[i].PoolHandleSubscriptions);
                //    //manualResetEventsArray[i].Set();
                //    i++;
                //}

                var actions = new List<Action>();

                Console.WriteLine($"Нашли {subscriptionQueue.Count} подписок");

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

                Console.WriteLine($"Обработка потоков началась в: {DateTime.Now.ToUniversalTime().ToLongTimeString()}");

                Parallel.Invoke(actionsArray);

                Console.WriteLine($"Обработка потоков завершена в: {DateTime.Now.ToUniversalTime().ToLongTimeString()}");

            }
            catch (Exception exception)
            {
                Console.WriteLine($"SearchSubscriptionJob: exception happend at UTC time: {DateTime.Now.ToUniversalTime().ToLongTimeString()}");
                Console.WriteLine($"{exception.Message}");
                Console.WriteLine("");
            }
            //try
            //{
            //    // Wait for all threads in pool to finish scanning.
            //    WaitHandle.WaitAll(manualResetEventsArray);
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
            Console.WriteLine($"SearchSubscriptionJob: Executed at UTC time: {DateTime.Now.ToUniversalTime().ToLongTimeString()}");
            Console.WriteLine("");
        }

    }
}
