using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using MediatR;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacancyCreateViewModel : PageViewModelBase
    {
        public VacancyCreateViewModel() { }

        public VacancyCreateViewModel(Guid organizationGuid)
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
        public bool ToPublish { get; set; }

        public VacancyStatus Status { get; set; }

        /// <summary>
        /// Инициализация справочников для формы
        /// </summary>
        /// <returns></returns>
        public void InitDictionaries(IMediator mediator)
        {
            if (mediator == null)
                throw new ArgumentNullException(nameof(mediator));
            if (OrganizationGuid == Guid.Empty)
                throw new NullReferenceException($"Property {nameof(OrganizationGuid)} has Empty value");


            PositionTypes = mediator.Send(new SelectAllPositionTypesQuery()).Select(c => new SelectListItem { Text = c.title, Value = c.id.ToString() });
            ResearchDirections = mediator.Send(new SelectAllResearchDirectionsQuery()).Select(c => new SelectListItem { Text = c.title, Value = c.id.ToString() });
            Regions = mediator.Send(new SelectAllRegionsQuery()).Select(c => new SelectListItem { Text = c.title, Value = c.id.ToString() });

            if (PositionTypeId != 0)
                PositionType = PositionTypes.Single(c => c.Value == PositionTypeId.ToString()).Text;
            if (ResearchDirectionId != 0)
                ResearchDirection = ResearchDirections.Single(c => c.Value == ResearchDirectionId.ToString()).Text;
            if (RegionId != 0)
                Region = Regions.Single(c => c.Value == RegionId.ToString()).Text;

            ContractTypes = new List<ContractType> { ContractType.Permanent, ContractType.FixedTerm }
                .Select(c => new SelectListItem { Value = ((int)c).ToString(), Text = c.GetDescription() });


            EmploymentTypes = new List<EmploymentType>
            {
               EmploymentType.Full,
               EmploymentType.Partial,
               EmploymentType.Probation,
               EmploymentType.Temporary,
               EmploymentType.Volunteering
            }
                .Select(c => new SelectListItem { Value = ((int)c).ToString(), Text = c.GetDescription() });

            OperatingScheduleTypes = new List<OperatingScheduleType>
            {
                OperatingScheduleType.Flexible,
                OperatingScheduleType.FullTime,
                OperatingScheduleType.Remote,
                OperatingScheduleType.Replacement,
                OperatingScheduleType.Rotation
            }
                .Select(c => new SelectListItem { Value = ((int)c).ToString(), Text = c.GetDescription() });

            //Заполнение иерархии
            if (CriteriasHierarchy.Count == 0)
            {
                CriteriasHierarchy =
                    mediator.Send(new SelectAllCriteriasQuery()).ToList().ToHierarchyCriteriaViewModelList(Criterias);
            }

        }



        /// <summary>
        /// Guid должности из справочника
        /// </summary>
        [Required(ErrorMessage = "Требуется выбрать Должность", AllowEmptyStrings = false)]
        [Range(1, int.MaxValue, ErrorMessage = "Требуется выбрать Должность")]
        public int PositionTypeId { get; set; }
        public string PositionType { get; set; }
        public IEnumerable<SelectListItem> PositionTypes { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        [Required(ErrorMessage = "Требуется заполнить поле Наименование")]
        public string Name { get; set; }

        /// <summary>
        /// Должность (Полное наименование)
        /// </summary>
        [Required(ErrorMessage = "Требуется заполнить поле Полное наименование")]
        public string FullName { get; set; }

        /// <summary>
        /// Отрасль науки
        /// </summary>
        public string ResearchDirection { get; set; }
        [Required(ErrorMessage = "Требуется выбрать Отрасль науки")]
        [Range(1, int.MaxValue, ErrorMessage = "Требуется выбрать Отрасль науки")]
        public int ResearchDirectionId { get; set; }
        public IEnumerable<SelectListItem> ResearchDirections { get; set; }

        /// <summary>
        /// Тематика исследований
        /// </summary>
        public string ResearchTheme { get; set; }

        /// <summary>
        /// Задачи
        /// </summary>
        [Required(ErrorMessage = "Требуется описать Задачи")]
        public string Tasks { get; set; }

        /// <summary>
        /// Зарплата в месяц
        /// </summary>
        [Required(ErrorMessage = "Укажите минимальную зарплату")]
        public int SalaryFrom { get; set; }
        [Required(ErrorMessage = "Укажите максимальную зарплату")]
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
        public ContractType ContractType
        {
            get { return (ContractType)ContractTypeValue; }
            set { ContractTypeValue = (int)value; }
        }
        public IEnumerable<SelectListItem> ContractTypes { get; set; }

        /// <summary>
        /// Срок трудового договора (для срочного договора)
        /// </summary>
        public decimal? ContractTime { get; set; }

        /// <summary>
        /// Срок трудового договора (для срочного договора)
        /// </summary>
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Вы ввели недопустимое значение. Допустимо от 0 до 100 лет.")]
        public decimal? ContractTimeYears
        {
            get { return Math.Truncate(ContractTime ?? 0); }
            set { ContractTime = ContractTime - Math.Truncate(ContractTime ?? 0) + value; }
        }
        /// <summary>
        /// Срок трудового договора (для срочного договора)
        /// </summary>
        [Range(minimum: 0, maximum: 11, ErrorMessage = "Вы ввели недопустимое значение. Допустимо от 0 до 11 месяцев.")]
        public decimal? ContractTimeMonths
        {
            get { return (ContractTime ??0) - Math.Truncate(ContractTime ?? 0); }
            set { ContractTime = Math.Truncate(ContractTime ?? 0) + value; }
        }

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
        [Required(ErrorMessage = "Укажите контактное лицо")]
        public string ContactName { get; set; }
        [Required(ErrorMessage = "Укажите E-mail")]
        [EmailAddress(ErrorMessage = "Поле E-mail содержит не допустимый адрес электронной почты.")]
        public string ContactEmail { get; set; }
        [Required(ErrorMessage = "Укажите номер телефона")]
        [Phone(ErrorMessage = "Поле Телефон содержит не допустимый номер телефона.")]
        public string ContactPhone { get; set; }
        public string ContactDetails { get; set; }


        [Required(ErrorMessage = "Требуется выбрать тип занятости")]
        public EmploymentType EmploymentType { get; set; }
        public IEnumerable<SelectListItem> EmploymentTypes { get; set; }

        [Required(ErrorMessage = "Требуется выбрать режим работы")]
        public OperatingScheduleType OperatingScheduleType { get; set; }
        public IEnumerable<SelectListItem> OperatingScheduleTypes { get; set; }

        [Required(ErrorMessage = "Требуется выбрать регион")]
        [Range(1, int.MaxValue, ErrorMessage = "Требуется выбрать регион")]
        public int RegionId { get; set; }
        public string Region { get; set; }
        public IEnumerable<SelectListItem> Regions { get; set; }

        public string CityName { get; set; }

        /// <summary>
        /// Притерии
        /// </summary>
        public List<CriteriaItemViewModel> CriteriasHierarchy { get; set; } = new List<CriteriaItemViewModel>();
        public List<VacancyCriteria> Criterias { get; set; }
    }
}