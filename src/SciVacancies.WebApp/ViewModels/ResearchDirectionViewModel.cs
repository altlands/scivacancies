using System.Collections.Generic;
using Microsoft.AspNet.Mvc.Rendering;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class ResearchDirectionViewModel: ViewModelBase
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string TitleEng { get; set; }
        public int Lft { get; set; }
        public int Rgt { get; set; }
        public int Lvl { get; set; }
        public string OecdCode { get; set; }
        public string WosCode { get; set; }
        public int Root { get; set; }

        public List<ResearchDirectionViewModel> Childs { get; set; }
        public bool ChildsContainers => Childs == null;

        public IEnumerable<SelectListItem> Items { get; set; }
    }
}
