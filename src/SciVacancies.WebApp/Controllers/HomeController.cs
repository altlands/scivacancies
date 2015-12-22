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
using Microsoft.Framework.Caching.Memory;

namespace SciVacancies.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        private readonly IOptions<CacheSettings> _cacheSettings;
        private MemoryCacheEntryOptions _cacheOptions
        {
            get
            {
                return new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(_cacheSettings.Value.MainPageExpiration));
            }
        }

        public HomeController(IMediator mediator, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
        {
            _mediator = mediator;
            _cache = cache;
            _cacheSettings = cacheSettings;
        }

        [PageTitle("Главная")]
        public IActionResult Index()
        {
            IndexViewModel model;
            model = new IndexViewModel { CurrentMediator = _mediator };

            PagedList<OrganizationDetailsViewModel> organizationsList;
            if (!_cache.TryGetValue<PagedList<OrganizationDetailsViewModel>>("first_organizations_by_vacancycount", out organizationsList))
            {
                var sourceOrganizations = _mediator.Send(new SelectPagedOrganizationsQuery
                {
                    PageSize = 4,
                    PageIndex = 1,
                    OrderBy = ConstTerms.OrderByVacancyCountDescending
                });
                if (sourceOrganizations != null)
                {
                    organizationsList = sourceOrganizations.MapToPagedList<Organization, OrganizationDetailsViewModel>();
                    _cache.Set<PagedList<OrganizationDetailsViewModel>>("first_organizations_by_vacancycount", organizationsList, _cacheOptions);
                }
            }
            model.OrganizationsList = organizationsList;

            PagedList<VacancyDetailsViewModel> vacanciesList;
            if (!_cache.TryGetValue<PagedList<VacancyDetailsViewModel>>("last_published_vacancies", out vacanciesList))
            {
                var sourceVacancy = _mediator.Send(new SelectPagedVacanciesQuery
                {
                    PageSize = 4,
                    PageIndex = 1,
                    OrderBy = ConstTerms.OrderByDateStartDescending,
                    PublishedOnly = true
                });
                if (sourceVacancy != null)
                {
                    vacanciesList = sourceVacancy.MapToPagedList<Vacancy, VacancyDetailsViewModel>();

                    //заполнить названия организаций
                    var organizationGuids = vacanciesList.Items.Select(c => c.OrganizationGuid).Distinct().ToList();
                    if (organizationGuids.Any())
                    {
                        var organizations = _mediator.Send(new SelectOrganizationsByGuidsQuery { OrganizationGuids = organizationGuids });
                        vacanciesList.Items.ForEach(c =>
                    {
                        var organization = organizations.SingleOrDefault(d => d.guid == c.OrganizationGuid);
                        if (organization != null)
                        {
                            c.OrganizationName = organization.name;
                        }
                    });
                    }
                    _cache.Set<PagedList<VacancyDetailsViewModel>>("last_published_vacancies", vacanciesList, _cacheOptions);
                }
            }
            model.VacanciesList = vacanciesList;

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
