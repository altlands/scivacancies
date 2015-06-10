using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Controllers;
using SciVacancies.WebApp.Engine.CustomAttribute;

namespace SciVacancies.WebApp.ViewComponents
{
	/// <summary>
	/// Поулчиьт спеисок закладок в личном кабинете
	/// </summary>
    public class ResearcherTabs : ViewComponent
    {
        public ResearcherTabs()
        {

            _tabs = new Dictionary<string, string>()
            ;

            //получить список закладок (названий методов), для которых указан атрибут NavigationTitle. 
            typeof (ResearchersController)
                .GetMethods()
                .Where(
                    prop => Attribute.IsDefined((MemberInfo) prop, typeof (NavigationTitleAttribute))
                )
                .ToList()
                .ForEach(prop =>
                    _tabs.Add(
                        (prop.IsDefined(typeof(ActionNameAttribute)) ? prop.GetCustomAttribute<ActionNameAttribute>().Name.ToLower() : prop.Name).ToLower()
                        , prop.GetCustomAttribute<NavigationTitleAttribute>().NavigationTitle)
                );
        }

        /// <summary>
        /// Закладки в личном кабинете
        /// </summary>
        private readonly Dictionary<string, string> _tabs;

        public IViewComponentResult Invoke()
        {

            return View("/Views/Partials/_ResearcherTabs", _tabs);
        }
    }
}
