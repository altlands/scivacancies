using System;

namespace SciVacancies.WebApp.Engine.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NavigationTitleAttribute : Attribute
    {
        public NavigationTitleAttribute(string navigationTitle)
        {
            NavigationTitle = navigationTitle;
        }
        public string NavigationTitle { get; set; }
    }
}

