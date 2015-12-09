using System.Collections.Generic;

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
        /*Common*/
        public bool SelectCommon { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PreviousLastName { get; set; }
        public string FirstNameEng { get; set; }
        public string MiddleNameEng { get; set; }
        public string LastNameEng { get; set; }
        public string PreviousLastNameEng { get; set; }
        public int ExtNumber { get; set; }
        public int BirthYear { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ScienceDegree { get; set; }
        public string ScienceRank { get; set; }

        /*Collections*/


        public List<ConferenceEditViewModel> Conferences { get; set; }
        public bool ConferencesChecked { get; set; }

        public List<RewardEditViewModel> Rewards { get; set; }
        public bool RewardsChecked { get; set; }
        public List<MembershipEditViewModel> Memberships { get; set; }
        public bool MembershipsChecked { get; set; }
        public List<EducationEditViewModel> Educations { get; set; }
        public bool EducationsChecked { get; set; }
        public List<PublicationEditViewModel> Publications { get; set; }
        public bool PublicationsChecked { get; set; }
        public List<InterestEditViewModel> Interests { get; set; }
        public bool InterestsChecked { get; set; }

        public List<ActivityEditViewModel> ResearchActivity { get; set; }
        public bool ResearchActivityChecked { get; set; }
        public List<ActivityEditViewModel> TeachingActivity { get; set; }
        public bool TeachingActivityChecked { get; set; }
        public List<ActivityEditViewModel> OtherActivity { get; set; }
        public bool OtherActivityChecked { get; set; }

    }
}
