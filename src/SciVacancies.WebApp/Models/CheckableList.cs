using System.Collections.Generic;

namespace SciVacancies.WebApp.Models
{
    public class CheckableList<T>: List<T>
    {
        public bool Checked { get; set; }
    }
}
