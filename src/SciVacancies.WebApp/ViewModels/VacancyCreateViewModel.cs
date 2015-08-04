﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MediatR;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using SciVacancies.Domain.Enums;
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

        public DateTime? InCommitteeDate { get; set; }

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

            if (Criterias.Count == 0)
            {
                Criterias.Add(
                  new CriteriaItemViewModel
                  {
                      Id = 1,
                      Title = "Результаты и востребованность научных исследований",
                      Items = new List<CriteriaItemViewModel>
                      {
                        new CriteriaItemViewModel { Id = 1, Count = 0, Title = "Web of Science"},
                        new CriteriaItemViewModel { Id = 2, Count = 0, Title = "Scopus"},
                        new CriteriaItemViewModel { Id = 3, Count = 0, Title = "Российский индекс научного цитирования"},
                        new CriteriaItemViewModel { Id = 4, Count = 0, Title = "Google Scholar "},
                        new CriteriaItemViewModel { Id = 5, Count = 0, Title = "ERIH (European Reference Index for the Humanities)" },
                        new CriteriaItemViewModel { Id = 6, Count = 0, Title = "Специализированная информационно-аналитическая система" }
                      }
                  });
                Criterias.Add(
                    new CriteriaItemViewModel
                    {
                        Id = 7,
                        Title = "Совокупная цитируемость публикаций",
                        Items = new List<CriteriaItemViewModel>
                            {
                            new CriteriaItemViewModel { Id = 8, Count = 0, Title = "Web of Science"},
                            new CriteriaItemViewModel { Id = 9, Count = 0, Title = "Scopus"},
                            new CriteriaItemViewModel { Id =10, Count = 0, Title = "Google Scholar"},
                            new CriteriaItemViewModel { Id =11, Count = 0, Title = "(Российский индекс научного цитирования)"}
                            }
                    });
                Criterias.Add(
                    new CriteriaItemViewModel
                    {
                        Id = 12,
                        Title = "Совокупный импакт-фактор журналов",
                        Items = new List<CriteriaItemViewModel>
                            {
                            new CriteriaItemViewModel { Id =13, Count = 0, Title = "(Значение)"},
                            }
                    });
                Criterias.Add(
                    new CriteriaItemViewModel
                    {
                        Id =14,
                        Title = "Общее количество научных, конструкторских и технологических произведений",
                        Items = new List<CriteriaItemViewModel>
                            {
                            new CriteriaItemViewModel { Id =15, Count = 0, Title = "опубликованных произведений"},
                            new CriteriaItemViewModel { Id =16, Count = 0, Title = "опубликованных периодических изданий"},
                            new CriteriaItemViewModel { Id =17, Count = 0, Title = "выпущенной конструкторской и технологической документации"},
                            new CriteriaItemViewModel { Id =18, Count = 0, Title = "неопубликованных произведений науки"}
                            }
                    });
                Criterias.Add(
                    new CriteriaItemViewModel
                    {
                        Id =19,
                        Title = "Количество созданных результатов интеллектуальной деятельности",
                        Items = new List<CriteriaItemViewModel>
                            {
                            new CriteriaItemViewModel { Id =20, Count = 0, Title = "учтенных в государственных информационных системах"},
                            new CriteriaItemViewModel { Id =21, Count = 0, Title = "имеющих государственную регистрацию и (или) правовую охрану в Российской Федерации"},
                            new CriteriaItemViewModel { Id =22, Count = 0, Title = "имеющих правовую охрану за пределами Российской Федерации"}
                            }
                    });
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
        public decimal ContractTime { get; set; }

        /// <summary>
        /// Срок трудового договора (для срочного договора)
        /// </summary>
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Вы ввели недопустимое значение. Допустимо от 0 до 100 лет.")]
        public decimal ContractTimeYears
        {
            get { return Math.Truncate(ContractTime); }
            set { ContractTime = ContractTime - Math.Truncate(ContractTime) + value; }
        }
        /// <summary>
        /// Срок трудового договора (для срочного договора)
        /// </summary>
        [Range(minimum: 0, maximum: 11, ErrorMessage = "Вы ввели недопустимое значение. Допустимо от 0 до 11 месяцев.")]
        public decimal ContractTimeMonths
        {
            get { return ContractTime - Math.Truncate(ContractTime); }
            set { ContractTime = Math.Truncate(ContractTime) + value; }
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
        public List<CriteriaItemViewModel> Criterias { get; set; } = new List<CriteriaItemViewModel>();
    }
}