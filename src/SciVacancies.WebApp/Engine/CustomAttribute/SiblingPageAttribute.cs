using System;

namespace SciVacancies.WebApp.Engine.CustomAttribute
{
    /// <summary>
    /// Методы в контроллере, помеченные этим атрибутом, можо собрать, например, в навигационную группу
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SiblingPageAttribute : Attribute
    {
        public SiblingPageAttribute() { }

        public SiblingPageAttribute(string navigationTitle)
        {
            NavigationTitle = navigationTitle;
        }
        public string NavigationTitle { get; set; }
    }
}

