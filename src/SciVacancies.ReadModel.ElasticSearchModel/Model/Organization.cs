using System;

namespace SciVacancies.ReadModel.ElasticSearchModel.Model
{
    public class Organization
    {
        /// <summary>
        /// Айдишник для поисковика
        /// </summary>            
        public Guid Id { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Полное наименование (на английском языке)
        /// </summary>
        public string NameEng { get; set; }

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Сокращенное наименование (на английском языке)
        /// </summary>
        public string ShortNameEng { get; set; }

        /// <summary>
        /// Населенный пункт
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Веб-сайт
        /// </summary>
        public string Website { get; set; }

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
        /// Организационно-правовая форма организации
        /// </summary>
        public string OrgForm { get; set; }

        /// <summary>
        /// Идентификатор организационно-правовой формы организации
        /// </summary>
        public int OrgFormId { get; set; }

        /// <summary>
        /// Количество опубликованных вакансий на данный момент
        /// </summary>
        public int PublishedVacancies { get; set; }

        /// <summary>
        /// ФОИВ
        /// </summary>
        public string Foiv { get; set; }

        /// <summary>
        /// Идентификатор ФОИВ
        /// </summary>
        public int FoivId { get; set; }

        /// <summary>
        /// Основной вид деятельности
        /// </summary>
        public string Activity { get; set; }

        /// <summary>
        /// Идентификатор основого вида деятельности
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// Имя руководителя
        /// </summary>
        public string HeadFirstName { get; set; }

        /// <summary>
        /// Фамилия руководителя
        /// </summary>
        public string HeadLastName { get; set; }

        /// <summary>
        /// Отчество руководителя
        /// </summary>
        public string HeadPatronymic { get; set; }

        /// <summary>
        /// Дата регистрация организации на сайте
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Дата последнего изменения профиля организации
        /// </summary>
        public DateTime? UpdateDate { get; set; }
    }
}
