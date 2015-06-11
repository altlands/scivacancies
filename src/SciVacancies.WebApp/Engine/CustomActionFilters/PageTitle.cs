using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp
{
    public class PageTitle: ActionFilterAttribute
    {
        public string Title;

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
