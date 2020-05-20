using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Helpers
{
    public static class CustomHelper
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controllers = null, string actions = null, string page = null, string cssClass = "active")
        {
            if(page != null)
            {
                string currentPage = htmlHelper.ViewContext.RouteData.Values["page"]?.ToString().Split("/").Last();
                string acceptedPage = page;

                return acceptedPage.Equals(currentPage) ? cssClass : String.Empty;
            }

            string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',');
            IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',');

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                cssClass : String.Empty;
        }
    }
}
