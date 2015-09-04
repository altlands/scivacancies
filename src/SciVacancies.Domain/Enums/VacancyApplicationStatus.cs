using System.ComponentModel;

namespace SciVacancies.Domain.Enums
{
    public enum VacancyApplicationStatus
    {
        /// <summary>
        /// Заявка на вакансию создана, но не отправлена
        /// </summary>
        [Description("В работе")]
        InProcess = 0,

        /// <summary>
        /// Заявка отправлена
        /// </summary>
        [Description("Отправлена")]
        Applied = 1,

        /// <summary>
        /// Заявка отменена (заявителем)
        /// </summary>
        [Description("Отменена")]
        Cancelled = 2,

        /// <summary>
        /// Заявка "выйграла" вакансию
        /// </summary>
        [Description("Победитель")]
        Won = 3,

        /// <summary>
        /// Заявка "получила" второе место
        /// </summary>
        [Description("Претендент")]
        Pretended = 4,

        /// <summary>
        /// Заявка "проиграла" конкурс
        /// </summary>
        [Description("Не в финале")]
        Lost = 5,

     
        /// <summary>
        /// Заявка со статусом "в работе" была удалена соискателем
        /// </summary>
        [Description("Удалена")]
        Removed = 6,

        
        ///// <summary>
        ///// отказ от предложения (победителем, либо организацией если победитель согласился)
        ///// </summary>
        //Refused=7
    }
}
