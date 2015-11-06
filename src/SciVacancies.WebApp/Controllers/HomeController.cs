using System.Linq;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.Models;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

using System;
using SciVacancies.ReadModel.Pager;
using Microsoft.Framework.OptionsModel;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Threading;

namespace SciVacancies.WebApp.Controllers
{
    [ResponseCache(NoStore = true)]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache cache;
        private readonly IOptions<CacheSettings> cacheSettings;

        public HomeController(IMediator mediator, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
        {
            _mediator = mediator;
            this.cache = cache;
            this.cacheSettings = cacheSettings;
        }

        //[ResponseCache(NoStore = true)]
        [PageTitle("Главная")]
        public IActionResult Index()
        {
            IndexViewModel model;
            //if (cache.TryGetValue("HomeIndexViewModel", out model))
            //{
            //    model = cache.Get<IndexViewModel>("HomeIndexViewModel");
            //}
            //else
            //{
            model = new IndexViewModel { CurrentMediator = _mediator };
            model.VacanciesList = _mediator.Send(new SelectPagedVacanciesQuery
            {
                PageSize = 4,
                PageIndex = 1,
                OrderBy = ConstTerms.OrderByDateStartDescending,
                PublishedOnly = true
            }).MapToPagedList<Vacancy, VacancyDetailsViewModel>();
            model.OrganizationsList = _mediator.Send(new SelectPagedOrganizationsQuery
            {
                PageSize = 4,
                PageIndex = 1,
                OrderBy = ConstTerms.OrderByVacancyCountDescending
            }).MapToPagedList<Organization, OrganizationDetailsViewModel>();

            //заполнить названия организаций
            if (model.VacanciesList != null)
            {
                var organizationGuids = model.VacanciesList.Items.Select(c => c.OrganizationGuid).Distinct().ToList();
                if (organizationGuids.Any())
                {
                    var organizations =
                        _mediator.Send(new SelectOrganizationsByGuidsQuery { OrganizationGuids = organizationGuids });
                    model.VacanciesList.Items.ForEach(c =>
                    {
                        var organization = organizations.SingleOrDefault(d => d.guid == c.OrganizationGuid);
                        if (organization != null)
                        {
                            c.OrganizationName = organization.name;
                        }
                    });
                }

            }

            //    cache.Set<IndexViewModel>("HomeIndexViewModel", model, new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddSeconds(cacheSettings.Value.ExpirationInSeconds)));
            //}

            ViewBag.IsMainPage = true;
            return View(model);
        }

        [PageTitle("Информация о системе")]
        public ActionResult About()
        {
            return View();
        }


    }
}
