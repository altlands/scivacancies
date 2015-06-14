using System;

namespace SciVacancies.WebApp.Engine.CustomAttribute
{
    /// <summary>
    /// Методы в контроллере, помеченные этим атрибутом, можно собрать, например, в навигационную группу.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SiblingPageAttribute : Attribute
    {
    }
}

