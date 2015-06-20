using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using PagedList;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp
{
    public class SearchController: Controller
    {
        private readonly IReadModelService _readModelService;

        public SearchController(IReadModelService readModelService)
        {
            _readModelService = readModelService;
        }


        /// <summary>
        /// фильтрация, страницы, сортировка 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PageTitle("Результаты поиска")]
        public ViewResult Index(VacanciesFilter model)
        {
            model.ValidateValues();

            //result data
            ViewBag.PagedData = _readModelService.SelectVacancies(
                ConstTerms.OrderByDateAscending /*model.OrderBy*/
                , model.PageSize
                , model.PageNumber
                );

            //dicitonaries
            ViewBag.FilterSource = new VacanciesFilterSource(_readModelService); //.SelectRegions(Guid.NewGuid());

            //search request term
            ViewBag.Search = model.Search;

            return View(model);
        }
    }
}
