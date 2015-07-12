using SciVacancies.Domain.Core;

using System;
using System.Collections.Generic;

namespace SciVacancies.Domain.DataModels
{
    public class ResearcherDataModel
    {
        public string FirstName { get; set; }
        public string FirstNameEng { get; set; }

        public string SecondName { get; set; }
        public string SecondNameEng { get; set; }

        public string Patronymic { get; set; }
        public string PatronymicEng { get; set; }

        public string FullName { get { return SecondName + " " + FirstName + " " + Patronymic; } }
        public string FullNameEng { get { return SecondNameEng + " " + FirstNameEng + " " + PatronymicEng; } }

        public string PreviousSecondName { get; set; }
        public string PreviousSecondNameEng { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }
        public string ExtraEmail { get; set; }

        public string Phone { get; set; }
        public string ExtraPhone { get; set; }

        public string Nationality { get; set; }

        public string ResearchActivity { get; set; }
        public string TeachingActivity { get; set; }
        public string OtherActivity { get; set; }

        public string ScienceDegree { get; set; }
        public string ScienceRank { get; set; }
        public string Rewards { get; set; }
        public string Memberships { get; set; }
        public string Conferences { get; set; }

        public List<Education> Educations { get; set; }
        public List<Publication> Publications { get; set; }

        public List<SearchSubscription> SearchSubscriptions { get; set; }

        public List<Guid> FavoriteVacancyGuids { get; set; }
    }
}
