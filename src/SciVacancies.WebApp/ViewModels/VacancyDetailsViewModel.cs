using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Nest;
using NPoco;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;
using SciVacancies.ReadModel.Pager;
using SciVacancies.WebApp.Interfaces;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacancyDetailsViewModel: ViewModelBase, IVacancyWinnerPretenderInfo
    {
        public List<VacancyApplication> AppliedByUserApplications { get; set; } = new List<VacancyApplication>();

        /// <summary>
        /// Идентификатор организации
        /// </summary>
        public Guid OrganizationGuid { get; set; }

        public Guid PositionGuid { get; set; }
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed, Type = FieldType.String)]
        public Guid PositionTypeGuid { get; set; }

        public string OrganizationName { get; set; }
        /// <summary>
        /// Должность
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Должность (Полное наименование)
        /// </summary>
        public string FullName { get; set; }


        /// <summary>
        /// Отрасль науки
        /// </summary>
        public ResearchDirection ResearchDirection { get; set; }
        public int ResearchDirectionId { get; set; }
        /// <summary>
        /// Тематика исследований
        /// </summary>
        public string ResearchTheme { get; set; }
        [Obsolete("Не приходит из ReadModel")]
        public int ResearchThemeId { get; set; }
        /// <summary>
        /// Задачи
        /// </summary>
        public string Tasks { get; set; }

        /// <summary>
        /// Зарплата в месяц
        /// </summary>
        public int SalaryFrom { get; set; }
        public int SalaryTo { get; set; }

        /// <summary>
        /// Стимулирующие выплаты
        /// </summary>
        public string Bonuses { get; set; }


        /// <summary>
        /// Тип трудового договора
        /// </summary>
        public ContractType ContractType { get; set; }

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
        /// Регион
        /// </summary>
        public string Region { get; set; }
        public int RegionId { get; set; }

        /// <summary>
        /// Населенный пункт 
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Дополнительно
        /// </summary>
        public string Details { get; set; }


        /// <summary>
        /// Лицо для получения дополнительных справок
        /// </summary>
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactDetails { get; set; }

        public VacancyStatus Status { get; set; }
        /// <summary>
        /// Количество добавивших вакансию в избранное
        /// </summary>
        public int FollowersCounter { get; set; }

        /// <summary>
        /// Дата начала публикации
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// Дата начала приёма заявок на вакансию
        /// </summary>
        public DateTime DateStartAcceptance { get; set; }
        /// <summary>
        /// Дата окончания приёма заявок на вакансию
        /// </summary>
        public DateTime DateFinishAcceptance { get; set; }
        /// <summary>
        /// Дата окончания публикации - когда вакансия переведена в статусы : отменена, закрыта
        /// </summary>
        public DateTime DateFinish { get; set; }
        [Obsolete("Дублируется с DateStart")]
        public DateTime CreationDate { get; set; }



        public DateTime InCommitteeDate { get; set; }
        public string InCommitteeDateString
        {
            get { return InCommitteeDate.ToString("dd.MM.yy"); }
            set { InCommitteeDate = DateTime.Parse(value, new CultureInfo("ru-RU")); }
        }

        /// <summary>
        /// Заявки на вакансию
        /// </summary>
        public PagedList<VacancyApplicationDetailsViewModel> Applications { get; set; }

        public ResearcherDetailsViewModel Winner { get; set; }
        public ResearcherDetailsViewModel Pretender { get; set; }

        public EmploymentType EmploymentType { get; set; }
        public OperatingScheduleType OperatingScheduleType { get; set; }
        public int PositionTypeId { get; set; }

        public bool? IsWinnerAccept { get; set; }
        public Guid WinnerResearcherGuid { get; set; }
        public DateTime? WinnerRequestDate { get; set; }
        public DateTime? WinnerResponseDate { get; set; }
        public Guid WinnerVacancyApplicationGuid { get; set; }

        public bool? IsPretenderAccept { get; set; }
        public Guid PretenderResearcherGuid { get; set; }
        public DateTime? PretenderRequestDate { get; set; }
        public DateTime? PretenderResponseDate { get; set; }
        public Guid PretenderVacancyApplicationGuid { get; set; }

        public IEnumerable<VacancyCriteria> Criterias { get; set; }
        public List<CriteriaItemViewModel> CriteriasHierarchy { get; set; }
        
        /// <summary>
        /// причина отмены
        /// </summary>
        public string CancelReason { get; set; }
        /// <summary>
        /// протокол закрытия
        /// </summary>
        public string CommitteeReasolution { get; set; }

        /// <summary>
        /// файлы, описывающие вакансию
        /// </summary>
        public List<VacancyAttachment> Attachments { get; set; }
        /// <summary>
        /// файлы решения комиссии
        /// </summary>
        public List<VacancyAttachment> AttachmentsCommittee { get; set; }

        /// <summary>
        /// часть пути для доступа к прикреплённым файлам
        /// </summary>
        public string FolderVacanciesAttachmentsUrl { get; set; }

        /// <summary>
        /// Autoincrimented field in DB - может быть null
        /// </summary>
        public long? ReadId { get; set; }
    }
}
