using System;
using System.Collections.Generic;
using SciVacancies.Domain.Core;

namespace SciVacancies.WebApp.Models.DataModels
{
    /// <summary>
    /// описание Исследователя (используоется для обновления информации об исследователе)
    /// </summary>
    public class ResearcherUpdateDataModel
    {

        public int ExtNumber { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        //TODO: переименовать в MiddleName/SecondName
        public string Patronymic { get; set; }
        //TODO: переименовать SecondName в LastName
        public string SecondName { get; set; }
        //TODO: переименовать PreviousSecondName в PreviousLastName
        public string PreviousSecondName { get; set; }

        public string FirstNameEng { get; set; }
        //TODO: переименовать в MiddleNameEng/SecondNameEng
        public string PatronymicEng { get; set; }
        //TODO: переименовать SecondNameEng в LastNameEng
        public string SecondNameEng { get; set; }
        //TODO: переименовать PreviousSecondNameEng в PreviousLastNameEng
        public string PreviousSecondNameEng { get; set; }

        public int BirthYear { get; set; }
        public DateTime? BirthDate { get; set; }

        public string Email { get; set; }
        [Obsolete("неопределено назначение этого свойства. планируется его удаление")]
        public string ExtraEmail { get; set; }
        public string Phone { get; set; }
        public string ExtraPhone { get; set; }

        /// <summary>
        /// научные интересы
        /// </summary>
        public string Interests { get; set; }

        /// <summary>
        /// временное поле для Образования, пока не перейдем на List[Education]
        /// </summary>
        public string Education { get; set; }

        public List<Publication> Publications { get; set; }

        /// <summary>
        /// награды
        /// </summary>
        public string Rewards { get; set; }

        public string Memberships { get; set; }

        [Obsolete("отказываемся от свойства Национальность")]
        public string Nationality { get; set; }
        //public string ResearchActivity { get; set; }
        //public string TeachingActivity { get; set; }
        //public string OtherActivity { get; set; }
        public string ScienceDegree { get; set; }
        public string ScienceRank { get; set; }
        public string Conferences { get; set; }
    }
}
