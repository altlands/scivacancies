using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.WebApp.Models.OAuth
{
    public class OAuthResProfile
    {
        public string birthday { get; set; }
        public List<string> degrees { get; set; }
        public OAuthResEducation education { get; set; }
        public string email { get; set; }
        public List<OAuthResEntity> entities { get; set; }
        public string firstName { get; set; }
        public string firstNameEn { get; set; }
        public List<OAuthResHonor> honors { get; set; }
        public int id { get; set; }
        public List<OAuthResInterest> interests { get; set; }
        public string lastName { get; set; }
        public string lastNameEn { get; set; }
        public string lastNameEnOld { get; set; }
        public string lastNameOld { get; set; }
        public string login { get; set; }
        public List<OAuthResMember> members { get; set; }
        public string middleName { get; set; }
        public string middleNameEn { get; set; }
        public string nationality { get; set; }
        public string phone { get; set; }
        public OAuthResPhoto photo { get; set; }
        public List<string> ranks { get; set; }
        public List<OAuthResAbstract> sAbstracts { get; set; }
    }
    public class OAuthResEducation
    {
        public string faculty { get; set; }
        public string org { get; set; }
        public string town { get; set; }
        public int year { get; set; }
    }
    public class OAuthResEntity
    {
        public string authors { get; set; }
        public int doi { get; set; }
        public string ext_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string updated { get; set; }
    }
    public class OAuthResHonor
    {
        public string agent { get; set; }
        public string deleted { get; set; }
        public string org { get; set; }
        public string title { get; set; }
        public string updated { get; set; }
        public int year { get; set; }
    }
    public class OAuthResIdentityNumberSc
    {
        public string intl { get; set; }
        public string scimap { get; set; }
    }
    public class OAuthResInterest
    {
        public string intName { get; set; }
        public string intNameEn { get; set; }
    }
    public class OAuthResMember
    {
        public string org { get; set; }
        public string position { get; set; }
        public string updated { get; set; }
        public int year_from { get; set; }
        public int year_to { get; set; }
    }
    public class OAuthResPhoto
    {
        public string format { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }
    public class OAuthResAbstract
    {
        public string category { get; set; }
        public string conference { get; set; }
        public string title { get; set; }
        public string updated { get; set; }
        public int year { get; set; }
    }
}
