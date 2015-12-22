using System.ComponentModel.DataAnnotations;

namespace SciVacancies.WebApp.ViewModels
{
    /// <summary>
    /// членство в профессиональных сообществах
    /// </summary>
    public class MembershipDetailsViewModel : MembershipEditViewModel
    {
    }

    /// <summary>
    /// членство в профессиональных сообществах
    /// </summary>
    public class MembershipEditViewModel
    {
        [MaxLength(1500, ErrorMessage = "ƒлина строки не более 1500 символов")]
        public string org { get; set; }
        [MaxLength(1500, ErrorMessage = "ƒлина строки не более 1500 символов")]
        public string position { get; set; }
        //public DateTime updated { get; set; }
        public int year_from { get; set; }
        public int year_to { get; set; }
    }
}