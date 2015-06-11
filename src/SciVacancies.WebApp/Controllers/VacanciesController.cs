﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using PagedList;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class VacanciesController : Controller
    {
        public VacanciesController()
        {

            var organization = new OrganizationItemViewModel { Guid = Guid.NewGuid(), Name = "Московский гусударственный университет имени М. В. Ломоносова" };
            var direction1 = new ScienceDirectionItemViewModel { Guid = Guid.NewGuid(), Name = "Физика" };
            var direction2 = new ScienceDirectionItemViewModel { Guid = Guid.NewGuid(), Name = "Механика" };
            var direction3 = new ScienceDirectionItemViewModel { Guid = Guid.NewGuid(), Name = "Инструменты и приборы" };
            var direction4 = new ScienceDirectionItemViewModel { Guid = Guid.NewGuid(), Name = "Компьютерная техника" };
            var direction5 = new ScienceDirectionItemViewModel { Guid = Guid.NewGuid(), Name = "Программирование" };
            var direction6 = new ScienceDirectionItemViewModel { Guid = Guid.NewGuid(), Name = "Экономика" };
            var direction7 = new ScienceDirectionItemViewModel { Guid = Guid.NewGuid(), Name = "Материаловедение – междисциплинарное" };
            var region = new RegionItemViewModel { Guid = Guid.NewGuid(), Name = "Москва" };

            var x = 0;
            var randomDate = new Random(1);
            var randomSalary = new Random(1000);
            while (x < 5)
            {

                data.Add(new VacanciesItemViewModel
                {
                    Guid = Guid.NewGuid(),
                    Title = "Инженер-исследователь",
                    PublishedDate = DateTime.Now.AddDays(randomDate.Next(-60, -30)),
                    MaxSalary = randomSalary.Next(50000, 90000),
                    MinSalary = randomSalary.Next(10000, 50000),
                    Organization = organization,
                    OrganizationGuid = organization.Guid,
                    Region = region,
                    RegionGuid = region.Guid,
                    ScienceDirections = new List<ScienceDirectionItemViewModel> { direction1, direction2, direction3, direction4, direction5, direction6, direction7 }
                });

                data.Add(new VacanciesItemViewModel
                {
                    Guid = Guid.NewGuid(),
                    Title = "Ведущий научный сотрудник",
                    PublishedDate = DateTime.Now.AddDays(randomDate.Next(-60, -30)),
                    MaxSalary = randomSalary.Next(50000, 90000),
                    MinSalary = randomSalary.Next(10000, 50000),
                    Organization = organization,
                    OrganizationGuid = organization.Guid,
                    Region = region,
                    RegionGuid = region.Guid,
                    ScienceDirections = new List<ScienceDirectionItemViewModel> { direction1, direction2, direction3 }
                });

                data.Add(new VacanciesItemViewModel
                {
                    Guid = Guid.NewGuid(),
                    Title = "Старший научный сотрудник",
                    PublishedDate = DateTime.Now.AddDays(randomDate.Next(-60, -30)),
                    MaxSalary = randomSalary.Next(50000, 90000),
                    MinSalary = randomSalary.Next(10000, 50000),
                    Organization = organization,
                    OrganizationGuid = organization.Guid,
                    Region = region,
                    RegionGuid = region.Guid,
                    ScienceDirections = new List<ScienceDirectionItemViewModel> { direction1, direction2, direction3 }
                });
                x++;
            }
        }

        private List<VacanciesItemViewModel> data = new List<VacanciesItemViewModel>();

        /// <summary>
        /// фильтрация, страницы, сортировка 
        /// </summary>
        /// <param name="salaries"></param>
        /// <param name="contestStates"></param>
        /// <param name="period"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderDescending"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ViewResult Search(
            List<int> salaries,
            List<int> contestStates,
            int period = 0 /* 0=all | 1=1 day |  7=7 days | 30=30 days */,
            string orderBy = "date" /* maxSalary */,
            bool orderDescending = true,
            int pageSize = 10,
            int page = 1
            )
        {
            IOrderedQueryable<VacanciesItemViewModel> orderedData;
            if (orderDescending)
                orderedData = data.AsQueryable().OrderByDescending(c => GetOrderingProperty(c, orderBy));
            else
                orderedData = data.AsQueryable().OrderBy(c => GetOrderingProperty(c, orderBy));

            var result = orderedData.ToPagedList(page, pageSize);

            return View(result);
        }

        private static object GetOrderingProperty(VacanciesItemViewModel c, string orderBy)
        {
            switch (orderBy.ToLower())
            {
                case "date":
                    return c.PublishedDate;
                default: // case "maxSalary":
                    return c.MaxSalary;
            }

        }

        public ViewResult Details(string id) => View();
    }
}
