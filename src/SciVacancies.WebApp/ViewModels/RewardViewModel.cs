using System;
using System.ComponentModel.DataAnnotations;

namespace SciVacancies.WebApp.ViewModels
{
    /// <summary>
    /// награда
    /// </summary>
    public class RewardDetailsViewModel : RewardEditViewModel
    {
    }


    /// <summary>
    /// награды
    /// </summary>
    public class RewardEditViewModel
    {
        //public string agent { get; set; }
        //public int deleted { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string org { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string title { get; set; }
        //public DateTime updated { get; set; }
        public int year { get; set; }
    }
}