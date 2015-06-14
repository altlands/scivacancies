using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp
{
    /// <summary>
    /// This attribute will set ViewBag.Title property
    /// </summary>
    public class PageTitle: ActionFilterAttribute
    {
        public string Title;
        /// <summary>
        /// This attribute will set ViewBag.Title property
        /// </summary>
        /// <param name="title"></param>
        public PageTitle(string title)
        {
            Title = title;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ((Controller) context.Controller).ViewBag.Title = Title;
        }
    }
}
