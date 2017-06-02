using System.Linq;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.OptionsModel;
using Newtonsoft.Json;
using NEventStore;
using NEventStore.Persistence;
using Quartz;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.Services.Quartz;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Infrastructure;
using SciVacancies.WebApp.Infrastructure.Saga;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly IStoreEvents _eventStore;
        private readonly IMediator _mediator;
        //readonly ISagaRepository _sagaRepository;
        //readonly ISchedulerService _schedulerService;
        //private readonly IOptions<SagaSettings> _sagaSettings;
        private readonly IOptions<Holidays> _holidays;

        public EventsController(
            IMediator mediator,
            IStoreEvents eventStore
            //ISagaRepository sagaRepository,
            //ISchedulerService schedulerService,
            //IOptions<SagaSettings> sagaSettings,
            //IOptions<Holidays> holidays
            )
        {
            //_sagaRepository = sagaRepository;
            _eventStore = eventStore;
            _mediator = mediator;
            //_schedulerService = schedulerService;
            //_sagaSettings = sagaSettings;
            //_holidays = holidays;

            //GetValue();
        }

        private void GetValue()
        {
            #region sagas
            //VacancySaga saga = _sagaRepository.GetById<VacancySaga>("vacancysaga", Guid.Parse("31a76215-c04c-4152-8848-baca1b7747ac"));

            //var job = new VacancySagaTimeoutJob
            //{
            //    SagaGuid = saga.Id
            //};

            //var details  = _schedulerService.GetTriggersOfJob(new JobKey(job.SagaGuid.ToString(), "VacancySagaTimeoutJob"));
            #endregion


            var dateFrom = new DateTime(2016, 02, 01);
            ////var dateFrom = new DateTime(2016,1,12);
            ////var dateTo= new DateTime(2015,11,28);

            var events = _eventStore.Advanced.GetFrom(dateFrom.ToUniversalTime());
            IList<NEventStore.ICommit> listOfCommit = events.ToList();
            ////var filtered = listOfCommit.Where(c=>c.CommitStamp >= dateFrom && c.CommitStamp <= dateFrom.AddDays(5)).ToList();
            var filtered = listOfCommit.Where(c => c.CommitStamp >= dateFrom /*&& c.CheckpointToken == "15302"*/).ToList();


            bool match;
            var pattern = "ngi@niig.su";
            //var someParsedGuid = Guid.Parse("352764fa-19de-4a60-b84e-b2ca290616ac"); //user Логин - pr.plunger@gmail.com
            //var someParsedGuid = Guid.Parse("8f474f2a-f185-4046-8957-252e6593b70a"); //vac guid 715
            var someParsedGuid = Guid.Parse("31b62a07-9166-4bdc-be49-fb1d00b40fa5"); //vac guid 709

            foreach (var commit in filtered)
            {
                if (commit.Events.Count > 0)
                {



                    foreach (var eventCommit in commit.Events)
                    {
                        var body = eventCommit.Body;
                        //if (body is ResearcherCreated)
                        //{
                        //    var parsed = body as ResearcherCreated;
                        //    if (parsed.ResearcherGuid == someParsedGuid)
                        //        match = true;
                        //    //var name = parsed.Data.Email;
                        //    //if (name.Contains(pattenr))
                        //    //    match = true;
                        //    //var eventsBody = body;
                        //}

                        //if (body is ResearcherUpdated)
                        //{
                        //    var parsed = body as ResearcherUpdated;
                        //    if (parsed.ResearcherGuid == someParsedGuid)
                        //        match = true;
                        //    //var name = parsed.Data.Email;
                        //    //if (name.Contains(pattenr))
                        //    //    match = true;
                        //    //var eventsBody = body;
                        //}

                        if (body is VacancyCreated)
                        {
                            var parsed = body as VacancyCreated;
                            var vacancyGuid = parsed.VacancyGuid;
                            if (vacancyGuid == someParsedGuid)
                                match = true;
                            var eventsBody = body;
                        }

                        //if (body is VacancySagaCreated)
                        //{
                        //    var parsed = body as VacancySagaCreated;
                        //    var vacancyGuid = parsed.VacancyGuid;
                        //    if (vacancyGuid == someParsedGuid)
                        //        match = true;
                        //    var eventsBody = body;
                        //}

                        if (body is VacancyPublished)
                        {
                            var parsed = body as VacancyPublished;
                            var vacancyGuid = parsed.VacancyGuid;
                            if (vacancyGuid == someParsedGuid)
                                match = true;
                            var eventsBody = body;
                        }

                        if (body is VacancyInCommittee)
                        {
                            var parsed = body as VacancyInCommittee;
                            var vacancyGuid = parsed.VacancyGuid;
                            if (vacancyGuid == someParsedGuid)
                                match = true;
                            var eventsBody = body;
                        }

                        if (body is VacancyCancelled)
                        {
                            var parsed = body as VacancyCancelled;
                            var vacancyGuid = parsed.VacancyGuid;
                            if (vacancyGuid == someParsedGuid)
                                match = true;
                            var eventsBody = body;
                        }

                        if (body is VacancyRemoved)
                        {
                            var parsed = body as VacancyRemoved;
                            var vacancyGuid = parsed.VacancyGuid;
                            if (vacancyGuid == someParsedGuid)
                                match = true;
                            var eventsBody = body;
                        }

                        //if (body is VacancyApplicationCreated)
                        //{
                        //    var parsed = body as VacancyApplicationCreated;
                        //    var name = parsed.Data.ResearcherFullName;
                        //    var applicationGuid = parsed.VacancyApplicationGuid;
                        //    var vacancyGuid = parsed.VacancyGuid;
                        //    if (name.Contains(pattenr))
                        //        match = true;
                        //    var eventsBody = body;
                        //}

                        //if (body is VacancyApplicationApplied)
                        //{
                        //    var parsed = body as VacancyApplicationApplied;
                        //    var applicationGuid = parsed.VacancyApplicationGuid;
                        //    var eventsBody = body;
                        //}

                        //if (body is VacancyApplicationCancelled)
                        //{
                        //    var parsed = body as VacancyApplicationCancelled;
                        //    var applicationGuid = parsed.VacancyApplicationGuid;
                        //    var eventsBody = body;
                        //}

                        //if (body is VacancyApplicationRemoved)
                        //{
                        //    var parsed = body as VacancyApplicationRemoved;
                        //    var applicationGuid = parsed.VacancyApplicationGuid;
                        //    var eventsBody = body;
                        //}

                        //if (body is VacancyInOfferResponseAwaitingFromWinner)
                        //{
                        //    var parsed = body as VacancyInOfferResponseAwaitingFromWinner;
                        //    var vacancyGuid = parsed.VacancyGuid;
                        //    var eventsBody = body;
                        //}

                        //if (body is VacancyCommitteeResolutionSet)
                        //{
                        //    var parsed = body as VacancyCommitteeResolutionSet;
                        //    var vacancyGuid = parsed.VacancyGuid;
                        //    var eventsBody = body;
                        //}

                        //if (body is VacancyApplicationWon)
                        //{
                        //    var parsed = body as VacancyApplicationWon;
                        //    var vacancyGuid = parsed.VacancyGuid;
                        //    var eventsBody = body;
                        //}

                        //if (body is VacancyApplicationPretended)
                        //{
                        //    var parsed = body as VacancyApplicationPretended;
                        //    var vacancyGuid = parsed.VacancyGuid;
                        //    var eventsBody = body;
                        //}
                    }
                }
            }

            //listOfCommit

            var test = 6;
        }

        public IActionResult Index()
        {
            return Content("Index");
        }

        //public IActionResult SetWinnerAcceptOfferStatus(Guid id)
        //{
        //    try
        //    {
        //        var preModel = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });
        //        _mediator.Send(new SetWinnerAcceptOfferCommand { VacancyGuid = preModel.vacancy_guid });
        //    }
        //    catch (Exception exception)
        //    {
        //        return Content($"Some exception happend: {exception.Message}");
        //    }
        //    return Content("Done");

        //}

        //public IActionResult CloseVacancyStatus(Guid id)
        //{
        //    try
        //    {
        //        _mediator.Send(new CloseVacancyCommand
        //        {
        //            VacancyGuid = id
        //        });
        //    }
        //    catch (Exception exception)
        //    {
        //        return Content($"Some exception happend: {exception.Message}");
        //    }
        //    return Content("Done");

        //}

        //public ContentResult FixPubs()
        //{
        //    var dateFrom = new DateTime(2015, 01, 13);
        //    var events = _eventStore.Advanced.GetFrom(DateTime.MinValue);
        //    IList<NEventStore.ICommit> listOfCommit = events.ToList();
        //    ////var filtered = listOfCommit.Where(c=>c.CommitStamp >= dateFrom && c.CommitStamp <= dateFrom.AddDays(5)).ToList();
        //    var filtered = listOfCommit.Where(c => c.CommitStamp >= dateFrom && c.CheckpointToken == "15302").ToList();

        //    var reseGuid = Guid.Parse("7192b382-d11e-4a79-8f22-513b4315cd56");

        //    foreach (var commit in filtered)
        //    {
        //        if (commit.Events.Count > 0)
        //        {
        //            foreach (var eventCommit in commit.Events)
        //            {
        //                var body = eventCommit.Body;
        //                if (body is ResearcherUpdated)
        //                {
        //                    var parsed = body as ResearcherUpdated;
        //                    if (parsed.ResearcherGuid == reseGuid)
        //                    {


        //                        /*get Edit*/
        //                        var preModel = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = reseGuid });
        //                        var model = Mapper.Map<ResearcherEditViewModel>(preModel);
        //                        IEnumerable<Education> educations =
        //                            _mediator.Send(new SelectResearcherEducationsQuery { ResearcherGuid = reseGuid });
        //                        if (educations != null && educations.Any())
        //                        {
        //                            model.Educations = Mapper.Map<List<EducationEditViewModel>>(educations);
        //                        }
        //                        IEnumerable<Publication> publications =
        //                            _mediator.Send(new SelectResearcherPublicationsQuery { ResearcherGuid = reseGuid });
        //                        if (publications != null && publications.Any())
        //                        {
        //                            model.Publications = Mapper.Map<List<PublicationEditViewModel>>(publications);
        //                        }


        //                        /*post Edit*/

        //                        var preModelPost = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = reseGuid });

        //                        var dataModel = Mapper.Map<ResearcherDataModel>(preModelPost);

        //                        var data = Mapper.Map(model, dataModel);
        //                        //маппинг игнорирует Индивидуальный номер учёного
        //                        //отдельно обновляем для "своих" пользователей Инд.Номер учёного, и не обновляем для "чужих" пользователей
        //                        if (!model.IsScienceMapUser)
        //                            data.ExtNumber = model.ExtNumber;
        //                        data.Publications = parsed.Data.Publications;

        //                        _mediator.Send(new UpdateResearcherCommand
        //                        {
        //                            Data = data,
        //                            ResearcherGuid = reseGuid
        //                        });

        //                    }
        //                }

        //            }

        //        }
        //    }

        //    return Content("cool");

        //}

        //[Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        ////[BindOrganizationIdFromClaims]
        //public ContentResult Recreate(/*Guid organizationGuid*/)
        //{
        //    var result = string.Empty;
        //    //var currentGuid = Guid.Parse("31a76215-c04c-4152-8848-baca1b7747ac");
        //    //var preModel = _mediator.Send(new SingleVacancyQuery {VacancyGuid = currentGuid});
        //    //preModel.criterias = _mediator.Send(new SelectVacancyCriteriasQuery { VacancyGuid = currentGuid }).ToList();
        //    //var model = Mapper.Map<VacancyCreateViewModel>(preModel);
        //    //model.CriteriasHierarchy = _mediator.Send(new SelectAllCriteriasQuery()).ToList().ToHierarchyCriteriaViewModelList(model.Criterias);

        //    ////correctoins
        //    //model.Guid = Guid.Empty;

        //    //var vacancyDataModel = Mapper.Map<VacancyDataModel>(model);
        //    //Organization organization = _mediator.Send(new SingleOrganizationQuery { OrganizationGuid = model.OrganizationGuid });
        //    //vacancyDataModel.OrganizationFoivId = organization.foiv_id;

        //    ////create command
        //    //var vacancyGuid = _mediator.Send(new CreateVacancyCommand { OrganizationGuid = model.OrganizationGuid, Data = vacancyDataModel });
        //    //result = vacancyGuid.ToString();
        //    /**/
        //    return Content(result);
        //}

        //[Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        ////[BindOrganizationIdFromClaims]
        //public ContentResult Republish(Guid id, int days/*, Guid organizationGuid*/)
        //{
        //    //public command
        //    //var inCommitteeDateValue = DateTime.Now.AddDays(days);
        //    //_mediator.Send(new PublishVacancyCommand
        //    //{
        //    //    VacancyGuid = id,
        //    //    InCommitteeStartDate = inCommitteeDateValue.Date,
        //    //    InCommitteeEndDate = inCommitteeDateValue.Date.AddMinutesIncludingHolidays(_sagaSettings.Value.Date.DeltaFromInCommitteeStartToEndMinutes, _holidays.Value.Dates)
        //    //});

        //    /**/
        //    return Content("done");
        //}

        public ContentResult test()
        {
            ////добавить инфу о победителе в вакансию
            //_mediator.Send(new SetVacancyWinnerCommand
            //{
            //    VacancyGuid = Guid.Parse("72477af2-63b4-4f29-b4ca-338e8f0b2ec5"),
            //    ResearcherGuid = Guid.Parse("0ad01966-63c7-4367-999b-d958c0d1d5e8"),
            //    VacancyApplicationGuid = Guid.Parse("ffbcdde4-9c8e-416c-9ed5-7495ae90aad0")
            //});

            ////переключить вакансию в статус ожидания ответа от победителя
            //_mediator.Send(new SetVacancyToResponseAwaitingFromWinnerCommand
            //{
            //    VacancyGuid = Guid.Parse("72477af2-63b4-4f29-b4ca-338e8f0b2ec5")
            //});

            _mediator.Send(new CancelVacancyCommand { VacancyGuid = Guid.Parse("73d456b8-570f-4192-83c8-65443113a6c3"), Reason = "Вакансия отменена по техническим причинам" });

            return Content("done");
        }

        public ContentResult CopyingApps(Guid id)
        {
            var result = string.Empty;


            //try
            //{
            //    if (_mediator.Send(new SingleVacancyQuery { VacancyGuid = id }) == null)
            //        return Content($"vacancy {id} not found");
            //}
            //catch (Exception exception)
            //{
            //    return Content(exception.Message);
            //}

            //var newVacancyGuid = id;
            //var vacancyGuid = Guid.Parse("31a76215-c04c-4152-8848-baca1b7747ac");

            //var page = _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
            //{
            //    PageSize = 1,
            //    PageIndex = 1,
            //    VacancyGuid = vacancyGuid,
            //    OrderBy = "Guid"
            //});
            //page = _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
            //{
            //    PageSize = page.TotalItems,
            //    PageIndex = 1,
            //    VacancyGuid = vacancyGuid,
            //    OrderBy = "Guid"
            //});
            //result += $"apps Count: {page.Items.Count} ; ";

            ////var applications = page.MapToPagedList<VacancyApplication, VacancyApplicationDetailsViewModel>();

            //try
            //{
            //    foreach (var vacancyApplication in page.Items)
            //    {
            //        var appliedVacancyApplications = _mediator.Send(new SelectVacancyApplicationsByResearcherQuery { ResearcherGuid = vacancyApplication.researcher_guid }).ToList();

            //        if (appliedVacancyApplications.Any(c => c.vacancy_guid == newVacancyGuid && c.status == VacancyApplicationStatus.Applied)
            //            //appliedVacancyApplications.Where(c => c.vacancy_guid == newVacancyGuid && c.status == VacancyApplicationStatus.Applied).Select(c => c.researcher_guid).Distinct().ToList().Any(c => c == vacancyApplication.researcher_guid)
            //            )
            //            continue;

            //        result += vacancyApplication.guid + " -> ";

            //        //скопировать прикреплённые файлы
            //        var vacancyApplicationAttachments = new List<SciVacancies.Domain.Core.VacancyApplicationAttachment>();
            //        var newFolderGuid = Guid.NewGuid();
            //        foreach (var attachment in _mediator.Send(new SelectAllVacancyApplicationAttachmentsQuery { VacancyApplicationGuid = vacancyApplication.guid }))
            //        {
            //            if (vacancyApplicationAttachments.Count == 0)
            //                result += $"(OldfolderGuid: {attachment.url.Split('/')[1]}) ";

            //            vacancyApplicationAttachments.Add(new SciVacancies.Domain.Core.VacancyApplicationAttachment
            //            {
            //                Size = attachment.size,
            //                Extension = attachment.extension,
            //                Name = attachment.name,
            //                UploadDate = attachment.upload_date,
            //                Url = attachment.url.Replace(attachment.url.Split('/')[1], newFolderGuid.ToString())
            //            });
            //        }
            //        result += $"(newfolderGuid: {newFolderGuid}) ";

            //        //скопировать заявку
            //        var data = new VacancyApplicationDataModel
            //        {
            //            Attachments = vacancyApplicationAttachments,
            //            BirthDate = vacancyApplication.birthdate,
            //            Conferences = vacancyApplication.conferences,
            //            CoveringLetter = vacancyApplication.covering_letter,
            //            Email = vacancyApplication.email,
            //            ExtraPhone = vacancyApplication.extraphone,
            //            Educations = !string.IsNullOrWhiteSpace(vacancyApplication.educations) ? JsonConvert.DeserializeObject<List<SciVacancies.Domain.Core.Education>>(vacancyApplication.educations) : new List<SciVacancies.Domain.Core.Education>(),
            //            Interests = vacancyApplication.interests,
            //            ImageUrl = vacancyApplication.image_url,
            //            Memberships = vacancyApplication.memberships,
            //            OtherActivity = vacancyApplication.other_activity,
            //            Publications = !string.IsNullOrWhiteSpace(vacancyApplication.publications) ? JsonConvert.DeserializeObject<List<SciVacancies.Domain.Core.Publication>>(vacancyApplication.publications) : new List<SciVacancies.Domain.Core.Publication>(),
            //            Phone = vacancyApplication.phone,
            //            PositionName = vacancyApplication.position_name,
            //            ResearchActivity = vacancyApplication.research_activity,
            //            ResearcherFullName = vacancyApplication.researcher_fullname,
            //            ResearcherFullNameEng = vacancyApplication.researcher_fullname_eng,
            //            Rewards = vacancyApplication.rewards,
            //            ScienceDegree = vacancyApplication.science_degree,
            //            ScienceRank = vacancyApplication.science_rank,
            //            TeachingActivity = vacancyApplication.teaching_activity,
            //        };

            //        //подать заявку
            //        var newVacancyApplicationGuid = _mediator.Send(new CreateVacancyApplicationTemplateCommand
            //        {
            //            ResearcherGuid = vacancyApplication.researcher_guid,
            //            VacancyGuid = newVacancyGuid,
            //            Data = data
            //        });
            //        _mediator.Send(new ApplyVacancyApplicationCommand
            //        {
            //            ResearcherGuid = vacancyApplication.researcher_guid,
            //            VacancyApplicationGuid = newVacancyApplicationGuid
            //        });

            //        result += $" (newGuid : {newVacancyApplicationGuid}) ; ";
            //    }
            //}
            //catch (Exception exception)
            //{
            //    result += $" {exception.Message} {exception}";
            //}

            return Content(result);
        }

        //public List<DateTime> Holidays()
        //{
        //    return _holidays.Value.Dates;
        //}
    }
}