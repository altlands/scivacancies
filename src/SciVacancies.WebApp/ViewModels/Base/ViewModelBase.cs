using System;
using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.ViewModels.Base
{
    public abstract class ViewModelBase
    {
        [HiddenInput]
        public Guid Guid { get; set; }
    }
}
