using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class OrganizationDetailsViewModel : PageViewModelBase
    {
        public OrganizationDetailsViewModel()
        {
            Title = "Карточка организации";
        }

        #region General

        /// <summary>
        /// Полное наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public string INN { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public string OGRN { get; set; }

        /// <summary>
        /// Имя руководителя
        /// </summary>
        public string HeadFirstName { get; set; }

        /// <summary>
        /// Фамилия руководителя
        /// </summary>
        public string HeadSecondName { get; set; }

        /// <summary>
        /// Отчество руководителя
        /// </summary>
        public string HeadPatronymic { get; set; }

        #endregion

        #region Dictionaries

        /// <summary>
        /// Наименование ФОИВ
        /// </summary>
        public string Foiv { get; set; }

        /// <summary>
        /// Идентификатор ФОИВ
        /// </summary>
        public int FoivId { get; set; }

        /// <summary>
        /// Наименование организационно-правовой формы
        /// </summary>
        public string OrgForm { get; set; }

        /// <summary>
        /// Идентификатор организационно-правовой формы
        /// </summary>
        public int OrgFormId { get; set; }

        /// <summary>
        /// Список идентификаторов отраслей науки
        /// </summary>
        public List<int> ResearchDirectionIds { get; set; }

        #endregion

        public OrganizationStatus Status { get; set; }

        public VacanciesInOrganizationIndexViewModel VacanciesInOrganization { get; set; }
    }
}
