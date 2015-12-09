using System;
using System.Globalization;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacancyPublishModel : ViewModelBase
    {

        /// <summary>
        /// Дата начала рассмотрения комиссии - используется при публикации 
        /// </summary>
        public DateTime InCommitteeDate { get; set; }
        public string InCommitteeDateString
        {
            get { return InCommitteeDate.ToString("dd.MM.yy"); }
            set { InCommitteeDate = DateTime.Parse(value, new CultureInfo("ru-RU")); }
        }

        public VacancyDetailsViewModel Details { get; set; }

    }
}
