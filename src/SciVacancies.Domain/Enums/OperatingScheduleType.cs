using System.ComponentModel;

namespace SciVacancies.Domain.Enums
{
    public enum OperatingScheduleType
    {
        /// <summary>
        /// Полный день
        /// </summary>
        [Description("Полный день")]
        FullTime = 0,

        /// <summary>
        /// Сменный график
        /// </summary>
        [Description("Сменный график")]
        Replacement = 1,

        /// <summary>
        /// Гибкий график
        /// </summary>
        [Description("Гибкий график")]
        Flexible = 2,

        /// <summary>
        /// Удалённая работа
        /// </summary>
        [Description("Удалённая работа")]
        Remote = 3,

        /// <summary>
        /// Вахтовый метод
        /// </summary>
        [Description("Вахтовый метод")]
        Rotation = 4
    }
}
