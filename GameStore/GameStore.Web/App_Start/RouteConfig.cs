using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("handler/{*path}");
            routes.MapRoute(
                name: "MakeOrder",
                url: "orders/new",
                defaults: new { controller = "Basket", action = "MakeOrder" });
            routes.MapRoute(
                name: "OrdersHistory",
                url: "orders/history",
                defaults: new { controller = "Order", action = "OrdersHistory" });
            routes.MapRoute(
                name: "OrderDetails",
                url: "orders/{id}",
                defaults: new { controller = "ManagerOrder", action = "OrderDetails", area = "Manager" })
                .DataTokens.Add("area", "Manager");
            routes.MapRoute(
              name: "ShowBasket",
              url: "basket/",
              defaults: new { controller = "Basket", action = "Index" });
            routes.MapRoute(
                name: "GetGamesByGenreId",
                url: "genres/{id}/games",
                defaults: new { controller = "Genre", action = "GetGamesByGenreId" });
            routes.MapRoute(
                name: "GenreDetails",
                url: "genres/{id}",
                defaults: new { controller = "Genre", action = "GenreDetails" });
            routes.MapRoute(
                name: "GetAllGenres",
                url: "genres/",
                defaults: new { controller = "Genre", action = "Index" });
            routes.MapRoute(
                name: "GetGamesByPublisherName",
                url: "publishers/{companyName}/games",
                defaults: new { controller = "Publisher", action = "GetGamesByPublisherName" });
            routes.MapRoute(
              name: "PublisherDetails",
              url: "publishers/{companyName}",
              defaults: new { controller = "Publisher", action = "PublisherDetails" });
            routes.MapRoute(
                name: "GetAllPublishers",
                url: "publishers/",
                defaults: new { controller = "Publisher", action = "Index" });
            routes.MapRoute(
                name: "GameImage",
                url: "games/image",
                defaults: new { controller = "Game", action = "RenderImage" });
            routes.MapRoute(
                name: "GameImageAsync",
                url: "games/image/async",
                defaults: new { controller = "Game", action = "RenderImageAsync" });
            routes.MapRoute(
                name: "GetGenresByGameKey",
                url: "games/{key}/genres",
                defaults: new { controller = "Game", action = "GetGenresByGameKey" });
            routes.MapRoute(
               name: "BuyProduct",
               url: "games/{gamekey}/buy",
               defaults: new { controller = "Basket", action = "BuyProduct" });
            routes.MapRoute(
                name: "DownloadGame",
                url: "games/{key}/download",
                defaults: new { controller = "Game", action = "DownloadGame" });
            routes.MapRoute(
                name: "GetByGameKey",
                url: "games/{key}/comments",
                defaults: new { controller = "Comment", action = "GetCommentsByGameKey" });
            routes.MapRoute(
               name: "GameDetails",
               url: "games/{key}",
               defaults: new { controller = "Game", action = "GameDetails" });
            routes.MapRoute(
                name: "GetAllGames",
                url: "games/",
                defaults: new { controller = "Game", action = "GetAllGames" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Game", action = "GetAllGames", id = UrlParameter.Optional });
        }
    }
}