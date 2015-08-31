using SciVacancies.WebApp.Models;

namespace SciVacancies.WebApp.ViewModels
{
    public class ResearcherProfileCompareModel
    {
        public ResearcherProfileCompareModelItem New { get; set; }
        public ResearcherProfileCompareModelItem Original { get; set; }

        public bool HasError { get; set; }
        private string _error;
        public string Error
        {
            get { return _error; }
            set
            {
                HasError = true;
                _error = value;
            }
        }
    }


    public class ResearcherProfileCompareModelItem
    {
        /*Names*/
        public bool SelectNames { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PreviousLastName { get; set; }
        public string FirstNameEng { get; set; }
        public string MiddleNameEng { get; set; }
        public string LastNameEng { get; set; }
        public string PreviousLastNameEng { get; set; }

        /*Numbers*/
        public bool SelectExtNumber { get; set; }
        public int ExtNumber { get; set; }

        public bool SelectBirthYear { get; set; }
        public int BirthYear { get; set; }


        /*Contacts*/
        public bool SelectEmail { get; set; }
        public string Email { get; set; }

        public bool SelectPhone { get; set; }
        public string Phone { get; set; }

        /*Activity*/

        public bool SelectResearchActivity { get; set; }
        public string ResearchActivity { get; set; }

        public bool SelectTeachingActivity { get; set; }
        public string TeachingActivity { get; set; }

        public bool SelectOtherActivity { get; set; }
        public string OtherActivity { get; set; }

        public bool SelectScienceDegree { get; set; }
        public string ScienceDegree { get; set; }

        public bool SelectScienceRank { get; set; }
        public string ScienceRank { get; set; }

        public bool SelectConferences { get; set; }
        public string Conferences { get; set; }

        /*Collections*/

        public CheckableItemsList<RewardEditViewModel> Rewards { get; set; }
        public CheckableItemsList<MembershipEditViewModel> Memberships { get; set; }
        public CheckableItemsList<EducationEditViewModel> Educations { get; set; }
        public CheckableItemsList<PublicationEditViewModel> Publications { get; set; }
        public CheckableItemsList<InterestEditViewModel> Interests { get; set; }

    }


}
