using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Areas.Manager.ViewModels;
using GameStore.Web.Controllers;
using Moq;
using Xunit;

namespace GameStore.Tests.ControllerTests
{
    public class GenreControllerTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly Mock<IGenreService> _genreServiceMock;
        private readonly GenreController _controller;

        public GenreControllerTests()
        {
            _gameServiceMock = new Mock<IGameService>();
            _genreServiceMock = new Mock<IGenreService>();
            var mapperMock = new Mock<IMapper>();
            _controller = new GenreController(_gameServiceMock.Object, _genreServiceMock.Object);
            mapperMock.Setup(m => m.Map<GenreDTO, GenreViewModel>(It.IsAny<GenreDTO>())).Returns(new GenreViewModel());
            mapperMock.Setup(m => m.Map<GenreViewModel, GenreDTO>(It.IsAny<GenreViewModel>())).Returns(new GenreDTO());
        }

        [Fact]
        public void GetGenresGet_SomeData_ReturnsViewResult()
        {
            var result = _controller.Index();

            Assert.NotNull(result);
        }
        
        [Fact]
        public void GenreDetails_NonExistentId_ReturnsRedirectResult()
        {
            _genreServiceMock.Setup(g => g.GetList()).Returns(new List<GenreDTO>
            {
                new GenreDTO { Id = 1, NameEn = "g1" }
            });

            var ex = Assert.Throws<HttpException>(() => _controller.GenreDetails(It.IsAny<int>()));

            Assert.NotNull(ex);
        }

        [Fact]
        public void GenreDetails_ExistentId_ReturnsViewResult()
        {
            _genreServiceMock.Setup(g => g.GetById(1)).Returns(new GenreDTO() { Id = 1, ParentGenreId = 2 });
            _genreServiceMock.Setup(g => g.GetById(2)).Returns(new GenreDTO() { Id = 2 });
            _gameServiceMock.Setup(g => g.GetByGenre(1)).Returns(Mock.Of<List<GameDTO>>());

            var result = _controller.GenreDetails(1) as ViewResult;

            Assert.NotNull(result);
        }
    }
}