using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.Web.Areas.Manager.Controllers;
using GameStore.Web.Areas.Manager.ViewModels;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class ManagerGenreControllerTests
    {
        private readonly Mock<IGenreService> _genreServiceMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly ManagerGenreController _controller;

        public ManagerGenreControllerTests()
        {
            _genreServiceMock = new Mock<IGenreService>();
            mapperMock = new Mock<IMapper>();
            _controller = new ManagerGenreController(_genreServiceMock.Object, mapperMock.Object);
            mapperMock.Setup(m => m.Map<GenreDTO, GenreViewModel>(It.IsAny<GenreDTO>())).Returns(new GenreViewModel());
            mapperMock.Setup(m => m.Map<GenreViewModel, GenreDTO>(It.IsAny<GenreViewModel>())).Returns(new GenreDTO());
        }

        [Fact]
        public void CreateGenreGet_NewGenre_ReturnsViewResult()
        {
            var result = _controller.CreateGenre();

            Assert.NotNull(result);
        }

        [Fact]
        public void CreateGenre_NewGenre_ReturnsRedirectResult()
        {
            _genreServiceMock.Setup(g => g.Add(It.IsAny<GenreDTO>()))
                .Returns(new OperationDetails(true));

            _controller.CreateGenre(new GenreViewModel { NameEn = "g1" });

            _genreServiceMock.Verify(srv => srv.Add(It.IsAny<GenreDTO>()), Times.Once);
        }

        [Fact]
        public void CreateGenre_ServiceError_ReturnsViewResult()
        {
            _genreServiceMock.Setup(g => g.Add(It.IsAny<GenreDTO>()))
                .Returns(new OperationDetails(false));

            _controller.CreateGenre(new GenreViewModel { NameEn = "g1" });

            _genreServiceMock.Verify(srv => srv.Add(It.IsAny<GenreDTO>()), Times.Once);
        }

        [Fact]
        public void EditGenreGet_NonExistentId_ReturnsRedirectResult()
        {
            _genreServiceMock.Setup(g => g.GetById(1))
                .Returns((GenreDTO)null);

            var ex = Assert.Throws<HttpException>(() => _controller.EditGenre(1));

            Assert.NotNull(ex);
        }

        [Fact]
        public void EditGenreGet_ExistentId_ReturnsViewResult()
        {
            _genreServiceMock.Setup(g => g.GetById(1))
                .Returns(new GenreDTO { Id = 1 });

            var result = _controller.EditGenre(1) as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void EditGenre_SomeGenreForEdit_ReturnsRedirectResult()
        {
            _genreServiceMock.Setup(g => g.Update(It.IsAny<GenreDTO>()))
                .Returns(new OperationDetails(true));

            _controller.EditGenre(new GenreViewModel());

            _genreServiceMock.Verify(srv => srv.Update(It.IsAny<GenreDTO>()), Times.Once);
        }

        [Fact]
        public void EditGenre_BadGenreObject_ReturnsViewResult()
        {
            _genreServiceMock.Setup(g => g.Update(It.IsAny<GenreDTO>()))
                .Returns(new OperationDetails(false, "Fail", null));

            _controller.EditGenre(new GenreViewModel());

            _genreServiceMock.Verify(srv => srv.Update(It.IsAny<GenreDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteGenreGet_NonExistentId_ReturnsRedirectResult()
        {
            _genreServiceMock.Setup(g => g.GetById(1))
                .Returns((GenreDTO)null);

            var ex = Assert.Throws<HttpException>(() => _controller.DeleteGenre(1));

            Assert.NotNull(ex);
        }

        [Fact]
        public void DeleteGenreGet_ExistentId_ReturnsViewResult()
        {
            _genreServiceMock.Setup(g => g.GetById(1))
                .Returns(new GenreDTO { Id = 1 });

            var result = _controller.DeleteGenre(1) as PartialViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteGenre_SomeGenreForDelete_ReturnsRedirectResult()
        {
            _genreServiceMock.Setup(g => g.Delete(It.IsAny<GenreDTO>()))
                .Returns(new OperationDetails(true));

            _controller.DeleteGenre(new DeleteGenreViewModel { Id = 1 });

            _genreServiceMock.Verify(srv => srv.Delete(It.IsAny<GenreDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteGenre_BadGenreObject_ReturnsRedirectResult()
        {
            _genreServiceMock.Setup(g => g.Delete(It.IsAny<GenreDTO>()))
                .Returns(new OperationDetails(false, "Fail", null));

            _controller.DeleteGenre(new DeleteGenreViewModel { Id = 1 });

            _genreServiceMock.Verify(srv => srv.Delete(It.IsAny<GenreDTO>()), Times.Once);
        }
    }
}