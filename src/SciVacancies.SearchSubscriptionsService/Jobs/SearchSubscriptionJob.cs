using System;
using SciVacancies.SmtpNotifications;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using Quartz;
using NPoco;
using Npgsql;
using Newtonsoft.Json;
using SciVacancies.Domain.Enums;
using SciVacancies.Services.SmtpNotificators;

namespace SciVacancies.SearchSubscriptionsService.Jobs
{
    public class SearchSubscriptionJob : IJob
    {
        static readonly string ServiceId = "#1";
        static readonly int Seed = 1;

        readonly IEmailService _emailService;
        //readonly IElasticService _elasticService;
        readonly IDatabase _db;
        private readonly ISmtpNotificatorSearchSubscriptionService _smtpNotificatorSearchSubscriptionService;

        //TODO
        public SearchSubscriptionJob(IEmailService emailService, IDatabase db, ISmtpNotificatorSearchSubscriptionService smtpNotificatorSearchSubscriptionService)
        {
            _emailService = emailService;
            //_elasticService = elasticService;
            _db = db;
            _smtpNotificatorSearchSubscriptionService = smtpNotificatorSearchSubscriptionService;
        }

        /// <summary>
        /// выполнить работу
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            ////
            var dataBase = new Database("Server = localhost; Database = scivacancies; User Id = postgres; Password = postgres", NpgsqlFactory.Instance);
            

            //TODO - загружать по частям, вызывая хранимую процедуру
            Queue<SciVacancies.ReadModel.Core.SearchSubscription> subscriptionQueue = new Queue<ReadModel.Core.SearchSubscription>(dataBase.Fetch<SciVacancies.ReadModel.Core.SearchSubscription>(new Sql($"SELECT * FROM res_searchsubscriptions ss WHERE ss.status = @0", SearchSubscriptionStatus.Active)));

            const int poolCount = 20;
            var threadSize = subscriptionQueue.Count > poolCount
                ? (subscriptionQueue.Count / poolCount) + 1
                : 1;
            var manualResetEventsArray = new ManualResetEvent[poolCount];
            var searchSubscriptionScannerArray = new SearchSubscriptionScanner[poolCount];

            var i = 0;
            while (subscriptionQueue.Skip(threadSize * i).Take(threadSize).Any())
            {
                manualResetEventsArray[i] = new ManualResetEvent(false);
                searchSubscriptionScannerArray[i] = new SearchSubscriptionScanner(manualResetEventsArray[i],_smtpNotificatorSearchSubscriptionService, subscriptionQueue.Skip(threadSize * i).Take(threadSize));
                //todo ntemnikov parallel linq
                ThreadPool.QueueUserWorkItem(searchSubscriptionScannerArray[i].PoolHandleSubscriptions);
                manualResetEventsArray[i].Set();
                i++;
            }

            // Wait for all threads in pool to finish scanning.
            WaitHandle.WaitAll(manualResetEventsArray);

        }

    }
}
