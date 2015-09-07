
namespace SciVacancies.WebApp.Models
{
    public class CheckableListItem<T>
    {
        public bool Checked { get; set; }

        public T This { get; set; }
    }
}