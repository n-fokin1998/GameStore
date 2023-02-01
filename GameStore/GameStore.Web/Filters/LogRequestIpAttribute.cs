using System.Web.Mvc;
using log4net;

namespace GameStore.Web.Filters
{
    public class LogRequestIpAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var logger = DependencyResolver.Current.GetService<ILog>();
            var ip = filterContext.HttpContext.Request.UserHostAddress;
            logger.Debug($"Class: [ { filterContext.ActionDescriptor.ControllerDescriptor.ControllerName } ] Request IP: [ { ip } ]");
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}