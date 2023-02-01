using System.Web;
using System.Web.Mvc;

namespace GameStore.Web.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return true;
            }

            return !httpContext.User.IsInRole("Administrator, Manager");
        }
    }
}