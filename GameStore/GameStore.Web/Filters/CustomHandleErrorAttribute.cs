using System;
using System.Web.Mvc;
using log4net;

namespace GameStore.Web.Filters
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute, IResultFilter
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var logger = DependencyResolver.Current.GetService<ILog>();
            var exception = filterContext.Exception;
            if (exception.GetType() == typeof(NotImplementedException))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Game" }, { "action", "GetAllGames" }, { "area", string.Empty }
                    });
                filterContext.ExceptionHandled = true;

                return;
            }

            if (!ExceptionType.IsInstanceOfType(exception))
            {
                return;
            }

            logger.Error("Error occured", exception);
            filterContext.HttpContext.Session["ErrorMessage"] = exception.Message;
            var url = filterContext.HttpContext.Session["Error"];
            if (url != null && url.ToString() != filterContext.HttpContext.Request.Url.AbsoluteUri)
            {
                filterContext.Result = new RedirectResult(url.ToString());
                filterContext.HttpContext.Session["Error"] = null;
            }
            else
            {
                filterContext.Result = new ViewResult { ViewName = "Error" };
            }

            filterContext.ExceptionHandled = true;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Session["Error"] = filterContext.HttpContext.Request.Url.AbsoluteUri;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }
    }
}