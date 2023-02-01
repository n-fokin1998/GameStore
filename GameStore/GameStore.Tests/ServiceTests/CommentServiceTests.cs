using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Services;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo.Abstract;
using Moq;
using Xunit;

namespace GameStore.Tests.ServiceTests
{
    public class CommentServiceTests
    {
        private readonly Mock<IRepository<Game>> _gameRepositoryMock;
        private readonly Mock<IRepository<Comment>> _commentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CommentService _commentService;

        public CommentServiceTests()
        {
            _gameRepositoryMock = new Mock<IRepository<Game>>();
            _commentRepositoryMock = new Mock<IRepository<Comment>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mongoUnitOfWorkMock = new Mock<IMongoUnitOfWork>();
            var mongoLoggerMock = new Mock<ILogRepository>();
            _mapperMock = new Mock<IMapper>();
            _commentService = new CommentService(unitOfWorkMock.Object, _mapperMock.Object);
            unitOfWorkMock.Setup(uow => uow.Comments).Returns(_commentRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Games).Returns(_gameRepositoryMock.Object);
            mongoUnitOfWorkMock.Setup(uow => uow.Logs).Returns(mongoLoggerMock.Object);
        }

        [Fact]
        public void GetCommentsByGameKey_NonExistentKey_ReturnsEmptyList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Comment>, List<CommentDTO>>(
                It.IsAny<IEnumerable<Comment>>())).Returns(new List<CommentDTO>());

            var result = _commentService.GetByGameKey(It.IsAny<string>());

            Assert.Empty(result);
        }

        [Fact]
        public void GetCommentsByGameKey_ExistingKey_ReturnsCommentList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Comment>, List<CommentDTO>>(It.IsAny<IEnumerable<Comment>>()))
                .Returns(new List<CommentDTO>
            {
                new CommentDTO() { Id = 1, GameId = 1 }
            });

            var result = _commentService.GetByGameKey("g1");

            Assert.Equal(1, result.FirstOrDefault().Id);
        }

        [Fact]
        public void AddComment_NullComment_ReturnFalseSucceeded()
        {
            var result = _commentService.Add(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddComment_NonExistenGame_ReturnFalseSucceeded()
        {
            var result = _commentService.Add(new CommentDTO { Id = 1, GameId = 1 });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddComment_NonExistentParentComment_ReturnFalseSucceeded()
        {
            _mapperMock.Setup(m => m.Map<CommentDTO, Comment>(It.IsAny<CommentDTO>()))
                .Returns(new Comment { Id = 1 });
            _gameRepositoryMock.Setup(g => g.GetItem(1)).Returns(Mock.Of<Game>());
            _gameRepositoryMock.Setup(g => g.GetItem(2)).Returns((Game)null);

            var result = _commentService.Add(new CommentDTO { Id = 1, GameId = 1, ParentCommentId = 2 });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddComment_SomeComment_ReturnTrueSucceeded()
        {
            _mapperMock.Setup(m => m.Map<CommentDTO, Comment>(It.IsAny<CommentDTO>()))
                .Returns(new Comment { Id = 1 });
            _gameRepositoryMock.Setup(g => g.GetItem(1)).Returns(Mock.Of<Game>());

            _commentService.Add(new CommentDTO { Id = 1, GameId = 1, ParentCommentId = null });

            _commentRepositoryMock.Verify(c => c.Add(It.IsAny<Comment>()), Times.Once);
        }

        [Fact]
        public void GetCommentById_NullId_ReturnsComment()
        {
            var result = _commentService.GetById(null);

            Assert.Null(result);
        }

        [Fact]
        public void GetCommentById_SomeId_ReturnsComment()
        {
            _mapperMock.Setup(m => m.Map<Comment, CommentDTO>(It.IsAny<Comment>()))
                .Returns(new CommentDTO { Id = 1 });
            _commentRepositoryMock.Setup(r => r.GetItem(1)).Returns(new Comment { Id = 1 });

            var result = _commentService.GetById(1);

            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void DeleteComment_NullComment_ReturnFalseSucceeded()
        {
            var result = _commentService.Delete(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteComment_NonExistenComment_ReturnFalseSucceeded()
        {
            var result = _commentService.Delete(new CommentDTO { Id = 1, Name = "c1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteComment_ExistingComment_ReturnTrueSucceeded()
        {
            _commentRepositoryMock.Setup(g => g.GetItem(1))
                .Returns(new Comment { Id = 1, Name = "c1", ParentCommentId = 2 });
            _commentRepositoryMock.Setup(c => c.GetList()).Returns(new List<Comment>
            {
                new Comment { Id = 1, Name = "c1", ParentCommentId = 2 },
                new Comment { Id = 2, Name = "c2" },
                new Comment { Id = 3, Name = "c3", ParentCommentId = 1 }
            }.AsQueryable());

            _commentService.Delete(new CommentDTO() { Id = 1, Name = "c1", ParentCommentId = 2 });

            _commentRepositoryMock.Verify(c => c.Update(It.IsAny<Comment>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void BanComment_SomeComment_ThrowsNotImplementedException()
        {
            var ex = Assert.Throws<NotImplementedException>(() => _commentService.Ban(
                It.IsAny<CommentDTO>(), It.IsAny<BanDuration>()));

            Assert.Equal(typeof(NotImplementedException), ex.GetType());
        }
    }
}