using System.Collections.Generic;
using System.Linq;
using SciVacancies.ReadModel.Core;

namespace SciVacancies
{
    public static class ListExtensions
    {
        /// <summary>
        /// выбрать протоколы решения комиссии
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<VacancyAttachment> SelectProtocols(this List<VacancyAttachment> source)
        {
            //TODO: реализовать после введения "типов" прикрепленных файлов
            return source/*.Where(c => c)*/.ToList();
        }

        /// <summary>
        /// выбрать файлы, описывающие вакансю (без протоколов и прочего)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<VacancyAttachment> SelectVacancyFiles(this List<VacancyAttachment> source)
        {
            //TODO: реализовать после введения "типов" прикрепленных файлов
            return source/*.Where(c => c)*/.ToList();
        }
    }
}
