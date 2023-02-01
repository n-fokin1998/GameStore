using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Abstract.Auth;

namespace GameStore.Web.Controllers
{
    public class BaseController : Controller
    {
        private const string CultureCookieName = "lang";
        private const string EnglishCulture = "en";

        public IAuthentication Auth { get; set; }

        public UserDTO CurrentUser => ((IUserIdentity)Auth.CurrentUser.Identity).User;

        public ActionResult ChangeLanguage(string lang, string url)
        {
            var langCookie = new HttpCookie(CultureCookieName)
            {
                HttpOnly = false,
                Value = lang,
                Expires = DateTime.Now.AddYears(1)
            };
            Response.Cookies.Add(langCookie);

            return Redirect(url);
        }

        public ActionResult LoginPatrial()
        {
            return PartialView(CurrentUser);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            var lang = requestContext.HttpContext.Request.Cookies[CultureCookieName] != null ?
                requestContext.HttpContext.Request.Cookies[CultureCookieName].Value : EnglishCulture;
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
            }
            catch (Exception)
            {
                throw new NotSupportedException($"Invalid language code '{lang}'.");
            }

            base.Initialize(requestContext);
        }
    }
}