//using System;
//using SciVacancies.SmtpNotifications;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Nest;
//using Quartz;
//using NPoco;
//using Npgsql;
//using Newtonsoft.Json;
//using SciVacancies.Domain.Enums;
//using SciVacancies.Services;
//using SciVacancies.Services.Email;
//using SciVacancies.SmtpNotifications.SmtpNotificators;

//namespace SciVacancies.SearchSubscriptionsService.Jobs
//{
//    public class SearchSubscriptionJob : IJob
//    {
//        static readonly string ServiceId = "#1";
//        static readonly int Seed = 1;

//        readonly IEmailService _emailService;
//        //readonly IElasticService _elasticService;
//        readonly IDatabase _db;
//        private readonly ISmtpNotificatorSearchSubscriptionService _smtpNotificatorSearchSubscriptionService;

//        //TODO
//        public SearchSubscriptionJob(IEmailService emailService, IDatabase db, ISmtpNotificatorSearchSubscriptionService smtpNotificatorSearchSubscriptionService, ISearchSubscriptionManager searchSubsciptionServiceManager)
//        {
//            _emailService = emailService;
//            //_elasticService = elasticService;
//            _db = db;
//            _smtpNotificatorSearchSubscriptionService = smtpNotificatorSearchSubscriptionService;
//            _searchSubsciptionServiceManager = searchSubsciptionServiceManager;
//        }

//        /// <summary>
//        /// выполнить работу
//        /// </summary>
//        /// <param name="context"></param>
//        public void Execute(IJobExecutionContext context)
//        {
//            
//            _searchSubsciptionServiceManager.Combine();
//            // Wait for all threads in pool to finish scanning.
//            WaitHandle.WaitAll(manualResetEventsArray);

//        }

//    }
//}
