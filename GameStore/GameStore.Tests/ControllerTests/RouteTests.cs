using GameStore.WebUI;
using Moq;
using System.Web;
using System.Web.Routing;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class RouteTests
    {
        private Mock<HttpContextBase> moqContext;
        private Mock<HttpRequestBase> moqRequest;
        private RouteCollection routes;

        public RouteTests()
        {
            moqContext = new Mock<HttpContextBase>();
            moqRequest = new Mock<HttpRequestBase>();
            routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            moqContext.Setup(x => x.Request).Returns(moqRequest.Object);
        }

        [Fact]
        public void RoutePublisherDetails_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/publisher/{CompanyName}");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Publisher", routeData.Values["controller"]);
            Assert.Equal("PublisherDetails", routeData.Values["action"]);
        }

        [Fact]
        public void RouteCreatePublisher_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/publishers/new");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Publisher", routeData.Values["controller"]);
            Assert.Equal("CreatePublisher", routeData.Values["action"]);
        }

        [Fact]
        public void RouteGetAllGames_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/games");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Game", routeData.Values["controller"]);
            Assert.Equal("GetAllGames", routeData.Values["action"]);
        }

        [Fact]
        public void RouteCreateGame_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/games/new");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Game", routeData.Values["controller"]);
            Assert.Equal("CreateGame", routeData.Values["action"]);
        }

        [Fact]
        public void RouteUpdateGame_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/games/update");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Game", routeData.Values["controller"]);
            Assert.Equal("EditGame", routeData.Values["action"]);
        }

        [Fact]
        public void RouteRemoveGame_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/games/remove");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Game", routeData.Values["controller"]);
            Assert.Equal("DeleteGame", routeData.Values["action"]);
        }

        [Fact]
        public void RouteGameDetails_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/game/g1");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Game", routeData.Values["controller"]);
            Assert.Equal("GameDetails", routeData.Values["action"]);
        }

        [Fact]
        public void RouteWriteComment_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/game/g1/newcomment");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Comment", routeData.Values["controller"]);
            Assert.Equal("WriteComment", routeData.Values["action"]);
        }

        [Fact]
        public void RouteGetCommentsByGameKey_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/game/g1/comments");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Comment", routeData.Values["controller"]);
            Assert.Equal("GetCommentsByGameKey", routeData.Values["action"]);
        }

        [Fact]
        public void RouteDownloadGame_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/game/g1/download");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Game", routeData.Values["controller"]);
            Assert.Equal("DownloadGame", routeData.Values["action"]);
        }

        [Fact]
        public void RouteBuyProduct_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/game/{gamekey}/buy");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Basket", routeData.Values["controller"]);
            Assert.Equal("BuyProduct", routeData.Values["action"]);
        }

        [Fact]
        public void RouteShowBasket_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/basket/");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Basket", routeData.Values["controller"]);
            Assert.Equal("Index", routeData.Values["action"]);
        }

        [Fact]
        public void RouteMakeOrder_RightUrl_ReturnsControllerAndActionName()
        {
            moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/order/");

            RouteData routeData = routes.GetRouteData(moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Basket", routeData.Values["controller"]);
            Assert.Equal("MakeOrder", routeData.Values["action"]);
        }
    }
}