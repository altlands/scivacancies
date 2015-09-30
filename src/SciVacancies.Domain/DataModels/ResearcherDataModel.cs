using SciVacancies.Domain.Core;
using System;
using System.Collections.Generic;

namespace SciVacancies.Domain.DataModels
{
    public class ResearcherDataModel
    {
        /// <summary>
        /// Идентификатор учёного в системе Карты Науки
        /// </summary>
        public string SciMapNumber { get; set; }

        /// <summary>
        /// Идентификационный номер Учёного
        /// </summary>
        public int ExtNumber { get; set; }

        public string FirstName { get; set; }
        public string FirstNameEng { get; set; }

        public string SecondName { get; set; }
        public string SecondNameEng { get; set; }

        public string Patronymic { get; set; }
        public string PatronymicEng { get; set; }

        public string FullName => SecondName + " " + FirstName + " " + Patronymic;
        public string FullNameEng => SecondNameEng + " " + FirstNameEng + " " + PatronymicEng;

        public string PreviousSecondName { get; set; }
        public string PreviousSecondNameEng { get; set; }

        public DateTime BirthDate { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }
        public string ExtraPhone { get; set; }

        [Obsolete("this property is unnecessary")]
        public string Nationality { get; set; }

        public string ResearchActivity { get; set; }
        public string TeachingActivity { get; set; }
        public string OtherActivity { get; set; }

        public string ScienceDegree { get; set; }
        public string ScienceRank { get; set; }

        /// <summary>
        /// научные интересы
        /// </summary>
        public string Interests { get; set; }

        public string Rewards { get; set; }
        public string Memberships { get; set; }
        public string Conferences { get; set; }

        /// <summary>
        /// Фотография исследователя
        /// </summary>
        public string ImageName { get; set; }
        public long? ImageSize { get; set; }
        public string ImageExtension { get; set; }
        public string ImageUrl { get; set; }

        public List<Education> Educations { get; set; } = new List<Education>();
        public List<Publication> Publications { get; set; } = new List<Publication>();
    }
}
