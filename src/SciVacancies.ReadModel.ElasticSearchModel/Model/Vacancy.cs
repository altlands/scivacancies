using System;
using Nest;
using SciVacancies.Domain.Enums;

namespace SciVacancies.ReadModel.ElasticSearchModel.Model
{
    public class Vacancy
    {
        /// <summary>
        /// Айдишник для поисковика
        /// </summary>       
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор организации
        /// </summary>
        public Guid OrganizationGuid { get; set; }

        /// <summary>
        /// Идентификатор позиции, на которую создана вакансия
        /// </summary>
        public Guid PositionGuid { get; set; }

        /// <summary>
        /// Тип позиции
        /// </summary>
        public string PositionType { get; set; }

        /// <summary>
        /// Идентификатор типа позиции (например "младший научный сотрудник")
        /// </summary>
        public int PositionTypeId { get; set; }

        //[Obsolete("Will be removed")]
        //[ElasticProperty(Index = FieldIndexOption.NotAnalyzed, Type = FieldType.String)]
        //public Guid PositionTypeGuid { get; set; }

        //public Guid WinnerGuid { get; set; }
        //public Guid PretenderGuid { get; set; }

        /// <summary>
        /// Полное имя организации, опубликовавшей вакансию
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Должность (Полное наименование)
        /// </summary>
        public string FullName { get; set; }

        public string Foiv { get; set; }
        public int FoivId { get; set; }

        /// <summary>
        /// Отрасль науки
        /// </summary>
        public string ResearchDirection { get; set; }

        /// <summary>
        /// Идентификатор отрасли науки
        /// </summary>
        public int ResearchDirectionId { get; set; }

        /// <summary>
        /// Тематика исследований
        /// </summary>
        public string ResearchTheme { get; set; }

        /// <summary>
        /// Идентификатои тематики исследований
        /// </summary>
        public int ResearchThemeId { get; set; }

        /// <summary>
        /// Задачи
        /// </summary>
        public string Tasks { get; set; }

        /// <summary>
        /// Зарплата в месяц от
        /// </summary>
        public int SalaryFrom { get; set; }

        /// <summary>
        /// Зарплата в месяц до
        /// </summary>
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

        /// <summary>
        /// Идентификатор региона
        /// </summary>
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
    }
}
