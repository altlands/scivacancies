using System.ComponentModel;

namespace SciVacancies.Domain.Enums
{
    public enum PositionStatus
    {
        /// <summary>
        /// В работе. По данной позиции опубликованной вакансии нет
        /// </summary>
        [Description("В работе")]
        InProcess = 0,
        /// <summary>
        /// По данной позиции опубликована вакансия, работа с позицией не возможна до закрытия или отмены вакансии
        /// </summary>
        [Description("Опубликована")]
        Published = 1,
        /// <summary>
        /// Позиция удалена и недоступна для организации (нужно создать новую)
        /// </summary>
        [Description("Удалена")]
        Removed = 2
    }
}
