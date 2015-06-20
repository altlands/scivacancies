using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class PositionCreateViewModel : PageViewModelBase
    {
        public PositionCreateViewModel() { }

        public PositionCreateViewModel(Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));
            OrganizationGuid = organizationGuid;
        }

        [HiddenInput]
        public Guid OrganizationGuid { get; set; }
        /// <summary>
        /// Сохранить как черновик (true) или сохранить и опубликовать данные (false)
        /// </summary>
        [HiddenInput]
        public bool ToPublish {
            get { return Status == PositionStatus.Published; }
            set { Status = value ? PositionStatus.Published : PositionStatus.InProcess; } }

        public PositionStatus Status { get; set; }

        /// <summary>
        /// Инициализация справочников для формы
        /// </summary>
        /// <param name="readModelService"></param>
        /// <returns></returns>
        public PositionCreateViewModel InitDictionaries(IReadModelService readModelService)
        {
            if (readModelService == null)
                throw new ArgumentNullException(nameof(readModelService));
            if (OrganizationGuid == Guid.Empty)
                throw new NullReferenceException($"Property {nameof(OrganizationGuid)} has Empty value");

            PositionTypes = readModelService.SelectPositionTypes().Select(c => new SelectListItem { Text = c.Title, Value = c.Guid.ToString() });
            ResearchDirections = readModelService.SelectResearchDirections().Select(c => new SelectListItem { Text = c.Title, Value = c.Guid.ToString() });

            ContractTypes = new List<ContractType>{ ContractType.Permanent, ContractType.FixedTerm}
                .Select(c => new SelectListItem { Value = ((int)c).ToString(), Text = c.GetDescription() });

            return this;
        }

        /// <summary>
        /// Guid должности из справочника
        /// </summary>
        public Guid PositionTypeGuid { get; set; }
        public IEnumerable<SelectListItem> PositionTypes { get; set; }

        /// <summary>
        /// Должность (Полное наименование)
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Отрасль науки
        /// </summary>
        public string ResearchDirection { get; set; }
        public int ResearchDirectionId { get; set; }
        public IEnumerable<SelectListItem> ResearchDirections { get; set; }

        /// <summary>
        /// Тематика исследований
        /// </summary>
        public string ResearchTheme { get; set; }

        /// <summary>
        /// Задачи
        /// </summary>
        public string Tasks { get; set; }

        /// <summary>
        /// Зарплата в месяц
        /// </summary>
        public int SalaryFrom { get; set; }
        public int SalaryTo { get; set; }
        //public Currency SalaryCurrency { get; set; }

        /// <summary>
        /// Стимулирующие выплаты
        /// </summary>
        public string Bonuses { get; set; }

        /// <summary>
        /// Дополнительно
        /// </summary>
        public string Details { get; set; }

        public int ContractTypeValue { get; set; }
        /// <summary>
        /// Тип трудового договора
        /// </summary>
        public ContractType ContractType {
            get { return (ContractType) ContractTypeValue; }
            set { ContractTypeValue = (int) value; }
        }
        public IEnumerable<SelectListItem> ContractTypes { get; set; }

        /// <summary>
        /// Срок трудового договора (для срочного договора)
        /// </summary>
        public decimal ContractTime { get; set; }

        /// <summary>
        /// Социальный пакет
        /// </summary>
        public bool SocialPackage { get; set; }

        /// <summary>
        /// Найм жилья
        /// </summary>
        public bool Rent { get; set; }

        /// <summary>
        /// Служебное жильё
        /// </summary>
        public bool OfficeAccomodation { get; set; }

        /// <summary>
        /// Компенсация транспорта
        /// </summary>
        public bool TransportCompensation { get; set; }

        /// <summary>
        /// Лицо для получения дополнительных справок
        /// </summary>
        public string ContactName { get; set; }
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Phone]
        public string ContactPhone { get; set; }
        public string ContactDetails { get; set; }

    }
}