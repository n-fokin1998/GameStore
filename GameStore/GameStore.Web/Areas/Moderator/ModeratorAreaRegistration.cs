using System.Web.Mvc;

namespace GameStore.Web.Areas.Moderator
{
    public class ModeratorAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Moderator";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "BanComment",
                url: "games/{key}/comments/{id}/ban",
                defaults: new { controller = "ModeratorComment", action = "BanComment" });
            context.MapRoute(
                name: "DeleteComment",
                url: "games/{key}/comments/{id}/remove",
                defaults: new { controller = "ModeratorComment", action = "DeleteComment" });
            context.MapRoute(
                "Moderator_default",
                "Moderator/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}