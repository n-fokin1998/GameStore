using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GameStore.Web;
using GameStore.Web.Areas.Admin;
using GameStore.Web.Areas.Manager;
using GameStore.Web.Areas.Moderator;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class RouteTests
    {
        private readonly Mock<HttpContextBase> _moqContext;
        private readonly Mock<HttpRequestBase> _moqRequest;
        private readonly RouteCollection _routes;

        public RouteTests()
        {
            _moqContext = new Mock<HttpContextBase>();
            _moqRequest = new Mock<HttpRequestBase>();
            _routes = new RouteCollection();
            RouteConfig.RegisterRoutes(_routes);
            _moqContext.Setup(x => x.Request).Returns(_moqRequest.Object);
        }

        [Fact]
        public void RoutePublisherDetails_RightUrl_ReturnsControllerAndActionName()
        {
            _moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/publishers/{CompanyName}");

            var routeData = _routes.GetRouteData(_moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Publisher", routeData.Values["controller"]);
            Assert.Equal("PublisherDetails", routeData.Values["action"]);
        }

        [Fact]
        public void RouteGetAllPublishers_RightUrl_ReturnsControllerAndActionName()
        {
            _moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/publishers/");

            var routeData = _routes.GetRouteData(_moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Publisher", routeData.Values["controller"]);
            Assert.Equal("Index", routeData.Values["action"]);
        }

        [Fact]
        public void RouteGetAllGames_RightUrl_ReturnsControllerAndActionName()
        {
            _moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/games");

            var routeData = _routes.GetRouteData(_moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Game", routeData.Values["controller"]);
            Assert.Equal("GetAllGames", routeData.Values["action"]);
        }

        [Fact]
        public void RouteGameDetails_RightUrl_ReturnsControllerAndActionName()
        {
            _moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/games/g1");

            var routeData = _routes.GetRouteData(_moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Game", routeData.Values["controller"]);
            Assert.Equal("GameDetails", routeData.Values["action"]);
        }

        [Fact]
        public void RouteGetCommentsByGameKey_RightUrl_ReturnsControllerAndActionName()
        {
            _moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/games/g1/comments");

            var routeData = _routes.GetRouteData(_moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Comment", routeData.Values["controller"]);
            Assert.Equal("GetCommentsByGameKey", routeData.Values["action"]);
        }

        [Fact]
        public void RouteDownloadGame_RightUrl_ReturnsControllerAndActionName()
        {
            _moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/games/g1/download");

            var routeData = _routes.GetRouteData(_moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Game", routeData.Values["controller"]);
            Assert.Equal("DownloadGame", routeData.Values["action"]);
        }

        [Fact]
        public void RouteBuyProduct_RightUrl_ReturnsControllerAndActionName()
        {
            _moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/games/{gamekey}/buy");

            var routeData = _routes.GetRouteData(_moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Basket", routeData.Values["controller"]);
            Assert.Equal("BuyProduct", routeData.Values["action"]);
        }

        [Fact]
        public void RouteShowBasket_RightUrl_ReturnsControllerAndActionName()
        {
            _moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/basket/");

            var routeData = _routes.GetRouteData(_moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Basket", routeData.Values["controller"]);
            Assert.Equal("Index", routeData.Values["action"]);
        }

        [Fact]
        public void RouteMakeOrder_RightUrl_ReturnsControllerAndActionName()
        {
            _moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/order/");

            var routeData = _routes.GetRouteData(_moqContext.Object);

            Assert.NotNull(routeData);
            Assert.Equal("Basket", routeData.Values["controller"]);
            Assert.Equal("MakeOrder", routeData.Values["action"]);
        }

        [Fact]
        public void ManagerAreaRegistration_SomeData_DontThrowException()
        {
            var managerAreaRegistration = new ManagerAreaRegistration();

            managerAreaRegistration.RegisterArea(new AreaRegistrationContext("Manager", new RouteCollection()));

            Assert.True(true);
        }

        [Fact]
        public void AdminAreaRegistration_SomeData_DontThrowException()
        {
            var adminAreaRegistration = new AdminAreaRegistration();

            adminAreaRegistration.RegisterArea(new AreaRegistrationContext("Admin", new RouteCollection()));

            Assert.True(true);
        }

        [Fact]
        public void ModeratorAreaRegistration_SomeData_DontThrowException()
        {
            var moderatorAreaRegistration = new ModeratorAreaRegistration();

            moderatorAreaRegistration.RegisterArea(new AreaRegistrationContext("Moderator", new RouteCollection()));

            Assert.True(true);
        }
    }
}