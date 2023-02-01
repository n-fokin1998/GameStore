using System.Web.Mvc;

namespace GameStore.Web.Areas.Manager
{
    public class ManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Manager";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "DeleteGenre",
                url: "genres/{id}/remove",
                defaults: new { controller = "ManagerGenre", action = "DeleteGenre" });
            context.MapRoute(
                name: "EditGenre",
                url: "genres/{id}/update",
                defaults: new { controller = "ManagerGenre", action = "EditGenre" });
            context.MapRoute(
                name: "CreateGenre",
                url: "genres/new",
                defaults: new { controller = "ManagerGenre", action = "CreateGenre" });
            context.MapRoute(
                name: "DeletePublisher",
                url: "publishers/{companyName}/remove",
                defaults: new { controller = "ManagerPublisher", action = "DeletePublisher" });
            context.MapRoute(
                name: "EditPublisher",
                url: "publishers/{companyName}/update",
                defaults: new { controller = "ManagerPublisher", action = "EditPublisher" });
            context.MapRoute(
                name: "CreatePublisher",
                url: "publishers/new",
                defaults: new { controller = "ManagerPublisher", action = "CreatePublisher" });
            context.MapRoute(
                name: "DeleteGame",
                url: "games/{key}/remove",
                defaults: new { controller = "ManagerGame", action = "DeleteGame" });
            context.MapRoute(
                name: "EditGame",
                url: "games/{key}/update",
                defaults: new { controller = "ManagerGame", action = "EditGame" });
            context.MapRoute(
                name: "CreateGame",
                url: "games/new",
                defaults: new { controller = "ManagerGame", action = "CreateGame" });
            context.MapRoute(
                name: "EditOrderDetail",
                url: "orders/{id}/update",
                defaults: new { controller = "ManagerOrder", action = "EditOrderDetail" });
            context.MapRoute(
                name: "OrdersManagement",
                url: "orders/",
                defaults: new { controller = "ManagerOrder", action = "Index" });
            context.MapRoute(
                "Manager_default",
                "Manager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}