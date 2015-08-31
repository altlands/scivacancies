using System.Collections.Generic;

namespace SciVacancies.WebApp.Models
{
    public class CheckableItemsList<T>: List<CheckableListItem<T>>
    {
        public bool Checked { get; set; }
    }
}
