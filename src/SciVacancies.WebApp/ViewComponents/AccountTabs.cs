using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Engine.CustomAttribute;

namespace SciVacancies.WebApp.ViewComponents
{
    /// <summary>
    /// Поулчиьт спеисок закладок в личном кабинете
    /// </summary>
    public class AccountTabs : ViewComponent
    {
        public IViewComponentResult Invoke(Type type)
        {
            var _tabs = new Dictionary<string, string>();

            //получить список закладок (названий методов), для которых указан атрибут NavigationTitle. 
            type
                .GetMethods()
                .Where(
                    prop => Attribute.IsDefined((MemberInfo)prop, typeof(SiblingPageAttribute))
                )
                .ToList()
                .ForEach(prop =>
                {
                    var pageTitleAttribute = prop.GetCustomAttribute<PageTitle>();
                        _tabs.Add(
                            (prop.IsDefined(typeof(ActionNameAttribute)) ? prop.GetCustomAttribute<ActionNameAttribute>().Name.ToLower() : prop.Name).ToLower()
                            , (pageTitleAttribute!=null && !string.IsNullOrEmpty(pageTitleAttribute.Title)) ? prop.GetCustomAttribute<PageTitle>().Title : prop.Name
                            );
                    }
                );
            return View("/Views/Partials/_AccountTabs", _tabs);
        }
    }
}
