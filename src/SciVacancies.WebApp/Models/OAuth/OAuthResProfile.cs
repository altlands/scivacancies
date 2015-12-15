using System;
using System.Collections.Generic;

namespace SciVacancies.WebApp.Models.OAuth
{
    public class OAuthResProfile
    {
        public IdentityNumberSc identityNumberSc { get; set; } = new IdentityNumberSc();

        public string birthday { get; set; }
        /// <summary>
        /// учёные степени
        /// </summary>
        public List<string> degrees { get; set; }
        public List<OAuthResEducation> education { get; set; }
        public string email { get; set; }
        /// <summary>
        /// научные публикации, ссылки
        /// </summary>
        public List<OAuthResEntity> entities { get; set; }
        public string firstName { get; set; }
        public string firstNameEn { get; set; }
        /// <summary>
        /// награды
        /// </summary>
        public List<OAuthResHonor> honors { get; set; }
        public int id { get; set; }
        /// <summary>
        /// научные интересы
        /// </summary>
        public List<OAuthResInterest> interests { get; set; }
        public string lastName { get; set; }
        public string lastNameEn { get; set; }
        public string lastNameEnOld { get; set; }
        public string lastNameOld { get; set; }
        public string login { get; set; }
        /// <summary>
        /// членство в профессиональных сообществах
        /// </summary>
        public List<OAuthResMember> members { get; set; }
        public string middleName { get; set; }
        public string middleNameEn { get; set; }
        public string phone { get; set; }
        public OAuthResPhoto photo { get; set; }
        /// <summary>
        /// учёные звания
        /// </summary>
        public List<string> ranks { get; set; }
        /// <summary>
        /// конференции(симпозиумы, конгрессы)
        /// </summary>
        public List<OAuthResAbstract> abstracts { get; set; }
        /// <summary>
        /// деятельность
        /// </summary>
        public List<OAuthResActivity> researches { get; set; }
    }
    public class OAuthResEducation
    {
        public string faculty { get; set; }
        public string org { get; set; }
        public string town { get; set; }
        public int year { get; set; }
    }
    /// <summary>
    /// научные публикации, ссылки
    /// </summary>
    public class OAuthResEntity
    {
        public string authors { get; set; }
        public string doi { get; set; }
        public string ext_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string updated { get; set; }
    }
    /// <summary>
    /// награда
    /// </summary>
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
    /// <summary>
    /// научные интересы
    /// </summary>
    public class OAuthResInterest
    {
        public string intName { get; set; }
        public string intNameEn { get; set; }
    }
    /// <summary>
    /// членство в профессиональных сообществах
    /// </summary>
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
        //TODO: где сама фотография
    }
    /// <summary>
    /// конференции (симпозиумы, конгрессы)
    /// </summary>
    public class OAuthResAbstract
    {
        public string categoryType { get; set; }
        public string conference { get; set; }
        public string title { get; set; }
        public string updated { get; set; }
        public int year { get; set; }
    }
    
    /// <summary>
    /// деятельность
    /// </summary>
    public class OAuthResActivity
    {
        public int yearFrom { get; set; }
        public int yearTo { get; set; }
        public string type { get; set; }
        public string organization{ get; set; }
        public string title { get; set; }
        public string position { get; set; }
    }

    public class IdentityNumberSc
    {
        public int scimap { get; set; }
    }
}
