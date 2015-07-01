using System;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.Enums;

namespace SciVacancies.WebApp.ViewModels
{
    public class PositionEditViewModel : PositionCreateViewModel
    {
        public PositionEditViewModel() { }

        public PositionEditViewModel(Guid organizationGuid): base(organizationGuid) { }



        /// <summary>
        /// Сохранить как черновик (true) или сохранить и опубликовать данные (false)
        /// </summary>
        [HiddenInput]
        public new bool ToPublish { get; set; }

    }
}