using System.ComponentModel;
using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    /// <summary>
    /// проверка недопустимых значений для Pager
    /// </summary>
    public class ValidatePagerParametersAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = ((Controller)context.Controller);

            var queryParam = "pageSize";
            UpdateArgument(context, controller, queryParam, 10);

            queryParam = "currentPage";
            UpdateArgument(context, controller, queryParam, 1);

            base.OnActionExecuting(context);
        }

        private static void UpdateArgument(ActionExecutingContext context, Controller controller, string queryParam, int defaultValue)
        {
            if (controller.Request.Query.ContainsKey(queryParam))
            {
                var attributeStringValue = controller.Request.Query[queryParam];
                var attributeNewValue = 0;
                if (int.TryParse(attributeStringValue, out attributeNewValue))
                {
                    if (attributeNewValue < 0)
                    {
                        context.ActionArguments[queryParam] = defaultValue;
                    }
                }
            }
        }
    }
}