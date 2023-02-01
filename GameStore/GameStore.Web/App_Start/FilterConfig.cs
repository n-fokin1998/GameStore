using System.Web.Mvc;
using GameStore.Web.Filters;

namespace GameStore.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
            filters.Add(new PerformanceTestAttribute());
            filters.Add(new LogRequestIpAttribute());
        }
    }
}