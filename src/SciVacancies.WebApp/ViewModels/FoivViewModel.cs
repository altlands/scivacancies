using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc.Rendering;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class FoivViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }

        public List<FoivViewModel> Childs { get; set; }
        public IEnumerable<SelectListItem> Items
        {
            get
            {
                return Childs?.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Title });
            }
        }
    }
}
