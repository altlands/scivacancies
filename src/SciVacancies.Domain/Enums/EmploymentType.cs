using System.ComponentModel;

namespace SciVacancies.Domain.Enums
{
    public enum EmploymentType
    {
        /// <summary>
        /// Полная занятость
        /// </summary>
        [Description("Полная занятость")]
        Full = 0,

        /// <summary>
        /// Частичная занятость
        /// </summary>
        [Description("Частичная занятость")]
        Partial = 1,

        /// <summary>
        /// Проектная/временная работа
        /// </summary>
        [Description("Временная работа")]
        Temporary = 2,

        /// <summary>
        /// Волонтёрство
        /// </summary>
        [Description("Волонтёрство")]
        Volunteering = 3,

        /// <summary>
        /// Стажировка
        /// </summary>
        [Description("Стажировка")]
        Probation = 4
    }
}
