using System;

namespace SciVacancies.WebApp.ViewModels
{
    public class PositionEditViewModel : PositionCreateViewModel
    {
        public PositionEditViewModel() { }

        public PositionEditViewModel(Guid organizationGuid): base(organizationGuid) { }
    }
}