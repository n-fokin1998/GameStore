using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Controllers;
using GameStore.Web.ViewModels;
using Moq;
using GameStore.BusinessLogicLayer.Infrastructure;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class CommentControllerTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly Mock<ICommentService> _commentServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CommentController _controller;

        public CommentControllerTests()
        {
            _gameServiceMock = new Mock<IGameService>();
            _commentServiceMock = new Mock<ICommentService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new CommentController(_gameServiceMock.Object, _commentServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void WriteComment_BadGameKey_ReturnsRedirectResult()
        {
            _gameServiceMock.Setup(g => g.GetByKey(It.IsAny<string>()))
                .Returns((GameDTO)null);

            var ex = Assert.Throws<HttpException>(() => _controller.WriteComment(It.IsAny<CommentViewModel>(), It.IsAny<string>()));

            Assert.NotNull(ex);
        }

        [Fact]
        public void WriteComment_SomeComment_ReturnsRedirectResult()
        {
            SetWriteComment();
            _commentServiceMock.Setup(c => c.Add(It.IsAny<CommentDTO>())).Returns(new OperationDetails(true));

            _controller.WriteComment(
                new CommentViewModel { Name = "a1", Body = "c1", ParentCommentId = 2 },
                It.IsAny<string>());

            _commentServiceMock.Verify(srv => srv.Add(It.IsAny<CommentDTO>()), Times.Once);
        }

        [Fact]
        public void WriteComment_ErrorInCommentService_ReturnsViewResult()
        {
            SetWriteComment();
            _commentServiceMock.Setup(c => c.Add(It.IsAny<CommentDTO>())).Returns(new OperationDetails(false));

            _controller.WriteComment(
                new CommentViewModel { Name = "a1", Body = "c1", ParentCommentId = 2 },
                It.IsAny<string>());

            _commentServiceMock.Verify(srv => srv.Add(It.IsAny<CommentDTO>()), Times.Once);
        }

        [Fact]
        public void GetCommentsByGameKey_SomeKey_ReturnsViewResultWithModel()
        {
            _gameServiceMock.Setup(g => g.GetByKey("g1"))
                .Returns(new GameDTO() { Id = 1, Key = "g1" });

            var result = _controller.GetCommentsByGameKey("g1") as ViewResult;
            var model = result.Model as CommentViewModel;

            Assert.Equal("g1", model.Game.Key);
        }

        [Fact]
        public void GetCommentsByGameKey_NonExistentKey_ReturnsViewResultWithModel()
        {
            _gameServiceMock.Setup(g => g.GetByKey("g1"))
                .Returns(Mock.Of<GameDTO>());

            var ex = Assert.Throws<HttpException>(() => _controller.GetCommentsByGameKey(It.IsAny<string>()));

            Assert.NotNull(ex);
        }

        private void SetWriteComment()
        {
            _mapperMock.Setup(m => m.Map<CommentViewModel, CommentDTO>(It.IsAny<CommentViewModel>())).Returns(
                new CommentDTO { ParentCommentId = 2, Body = "c1" });
            _gameServiceMock.Setup(g => g.GetByKey(It.IsAny<string>()))
                .Returns(new GameDTO { Id = 1 });
            _commentServiceMock.Setup(g => g.GetById(2))
                .Returns(new CommentDTO { Name = "Test" });
        }
    }
}