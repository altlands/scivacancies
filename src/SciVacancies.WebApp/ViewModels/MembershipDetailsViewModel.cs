using System;

namespace SciVacancies.WebApp.ViewModels
{
    /// <summary>
    /// членство в профессиональных сообществах
    /// </summary>
    public class MembershipDetailsViewModel
    {
        public string org { get; set; }
        public string position { get; set; }
        public DateTime updated { get; set; }
        public int year_from { get; set; }
        public int year_to { get; set; }
    }
}