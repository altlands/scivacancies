using System;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.Controllers
{
    public class ApplicationsController: Controller
    {
        [PageTitle("Новая заявка")]
        public ViewResult Create(Guid researcherGuid, Guid vacancyGuid)
        {
            return View(new PageViewModelBase());
        }
    }
}
