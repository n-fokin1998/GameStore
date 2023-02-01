using System.Web.Mvc;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Areas.Moderator.Controllers;
using GameStore.Web.Areas.Moderator.ViewModels;
using GameStore.Web.ViewModels.Enums;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class ModeratorCommentControllerTests
    {
        private readonly Mock<ICommentService> _commentServiceMock;
        private readonly ModeratorCommentController _controller;

        public ModeratorCommentControllerTests()
        {
            _commentServiceMock = new Mock<ICommentService>();
            _controller = new ModeratorCommentController(_commentServiceMock.Object);
        }

        [Fact]
        public void DeleteCommentGet_NonExistentComment_ReturnsRedirectResult()
        {
            var result = _controller.DeleteComment(It.IsAny<int>(), It.IsAny<string>()) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteCommentGet_SomeComment_ReturnsViewResult()
        {
            _commentServiceMock.Setup(c => c.GetById(It.IsAny<int>())).Returns(new CommentDTO { Id = 1 });

            var result = _controller.DeleteComment(It.IsAny<int>(), It.IsAny<string>()) as PartialViewResult;

            Assert.NotNull(result.Model);
        }

        [Fact]
        public void DeleteComment_SomeViewModel_ReturnsRedirectResult()
        {
            _controller.DeleteComment(new DeleteCommentViewModel { Id = 1, GameKey = "g1" });

            _commentServiceMock.Verify(srv => srv.Delete(It.IsAny<CommentDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteComment_NullCommentId_ReturnsRedirectResult()
        {
            var result = _controller.DeleteComment(
                new DeleteCommentViewModel { Id = null, GameKey = "g1" }) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void BanCommentGet_NonExistentComment_ReturnsRedirectResult()
        {
            var result = _controller.BanComment(It.IsAny<int>(), It.IsAny<string>()) as RedirectToRouteResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void BanCommentGet_SomeComment_ReturnsViewResult()
        {
            _commentServiceMock.Setup(c => c.GetById(It.IsAny<int>())).Returns(new CommentDTO { Id = 1 });

            var result = _controller.BanComment(It.IsAny<int>(), It.IsAny<string>()) as ViewResult;

            Assert.NotNull(result.Model);
        }

        [Fact]
        public void BanComment_SomeViewModel_ThrowsNotImplementedException()
        {
            _controller.BanComment(new BanCommentViewModel { Id = 1, Duration = BanDurationViewModel.OneDay });

            _commentServiceMock.Verify(srv => srv.Ban(It.IsAny<CommentDTO>(), It.IsAny<BanDuration>()), Times.Once);
        }
    }
}