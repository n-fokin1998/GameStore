using System.Diagnostics;
using System.Web.Mvc;
using log4net;

namespace GameStore.Web.Filters
{
    public class PerformanceTestAttribute : FilterAttribute, IActionFilter
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var logger = DependencyResolver.Current.GetService<ILog>();
            _stopwatch.Stop();
            var executionTime = _stopwatch.ElapsedMilliseconds;
            logger.Debug($"Class: [ { filterContext.ActionDescriptor.ControllerDescriptor.ControllerName }" +
                         $" ] Execution time for { filterContext.ActionDescriptor.ActionName } : [ { executionTime } ms.]");
        }
    }
}