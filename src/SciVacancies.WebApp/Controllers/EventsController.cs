//using System.Linq;
//using Microsoft.AspNet.Mvc;
//using System;
//using System.Collections.Generic;
//using NEventStore;
//using NEventStore.Persistence;
//using SciVacancies.Domain.Events;

//namespace SciVacancies.WebApp.Controllers
//{
//    public class EventsController: Controller
//    {
//        private IStoreEvents _eventStore;

//        public EventsController(IStoreEvents eventStore)
//        {
//            _eventStore = eventStore;
//            var events = _eventStore.Advanced.GetFrom(DateTime.MinValue);
//            IList<NEventStore.ICommit> listOfCommit = events.ToList();

//            var dateFrom = new DateTime(2015, 11, 01);
//            //var dateFrom = new DateTime(2016,1,12);
//            //var dateTo= new DateTime(2015,11,28);



//            bool match;
//            var nameContains = "Макаров";


//            //var filtered = listOfCommit.Where(c=>c.CommitStamp >= dateFrom && c.CommitStamp <= dateFrom.AddDays(5)).ToList();
//            var filtered = listOfCommit.Where(c => c.CommitStamp >= dateFrom).ToList();
//            foreach (var commit in filtered)
//            {
//                if (commit.Events.Count > 0)
//                {
//                    foreach (var eventCommit in commit.Events)
//                    {
//                        var body = eventCommit.Body;
//                        if (body is ResearcherCreated)
//                        {
//                            var parsed = body as ResearcherCreated;
//                            var name = parsed.Data.FullName;
//                            if (name.Contains(nameContains))
//                                match = true;
//                            var eventsBody = body;
//                        }

//                        if (body is ResearcherUpdated)
//                        {
//                            var parsed = body as ResearcherUpdated;
//                            var name = parsed.Data.FullName;
//                            if (name.Contains(nameContains))
//                                match = true;
//                            var eventsBody = body;
//                        }

//                        if (body is VacancyApplicationCreated)
//                        {
//                            var parsed = body as VacancyApplicationCreated;
//                            var name = parsed.Data.ResearcherFullName;
//                            var applicationGuid = parsed.VacancyApplicationGuid;
//                            var vacancyGuid = parsed.VacancyGuid;
//                            if (name.Contains(nameContains))
//                                match = true;
//                            var eventsBody = body;
//                        }

//                        if (body is VacancyApplicationApplied)
//                        {
//                            var parsed = body as VacancyApplicationApplied;
//                            var applicationGuid = parsed.VacancyApplicationGuid;
//                            var eventsBody = body;
//                        }

//                        if (body is VacancyApplicationCancelled)
//                        {
//                            var parsed = body as VacancyApplicationCancelled;
//                            var applicationGuid = parsed.VacancyApplicationGuid;
//                            var eventsBody = body;
//                        }

//                        if (body is VacancyApplicationRemoved)
//                        {
//                            var parsed = body as VacancyApplicationRemoved;
//                            var applicationGuid = parsed.VacancyApplicationGuid;
//                            var eventsBody = body;
//                        }
//                    }
//                }
//            }

//            //listOfCommit

//            var test = 6;
//        }

//        public IActionResult Index()
//        {
//            return Content("temp");
//        }

//    }
//}