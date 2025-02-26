using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.AccessControl;
using SpiceAndShine.Models;
using SpiceAndShine.Areas.Manager.Models;

namespace SpiceAndShine.Areas.Manager.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (HttpContext.Session.GetComplexData<ManagerSession>(Common.SessionKeys.ManagerSession) == null)
            {
                // Custome Error Code for session timeout on ajax request 
                if (IsAjaxRequest(filterContext.HttpContext.Request))
                {
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                    { "Controller", "Login" },
                                    { "Action", "SessionOut" }
                        });
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                    { "Controller", "Login" },
                                    { "Action", "Index" }
                        });
                }
            }
            else
            {
                ManagerSession managerSession = HttpContext.Session.GetComplexData<ManagerSession>(Common.SessionKeys.ManagerSession);
            }
        }

        public ManagerSession GetCurrentRestaurant()
        {
            return HttpContext.Session.GetComplexData<ManagerSession>(Common.SessionKeys.ManagerSession);
        }

        public bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
            {
                return false;
            }
            if (request.Headers == null)
            {
                return false;
            }
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
