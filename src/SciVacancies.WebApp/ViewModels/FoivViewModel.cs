using System.Collections.Generic;
using Microsoft.AspNet.Mvc.Rendering;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class FoivViewModel: ViewModelBase
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }

        public List<FoivViewModel> Childs { get; set; }
        public bool ChildsContainers => Childs == null;

        public IEnumerable<SelectListItem> Items { get; set; }
    }
}
