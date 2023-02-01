using System;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo.Abstract;
using GameStore.Domain.Repositories;
using Moq;
using Xunit;

namespace GameStore.Tests.DomainTests
{
    public class UnitOfWorkTests
    {
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            var contextMock = new Mock<IDbContext>();
            var logRepository = new Mock<ILogRepository>();
            var gameRepository = new Lazy<IRepository<Game>>(() 
                => new GameRepository(contextMock.Object, logRepository.Object));
            var commentRepository = new Lazy<IRepository<Comment>>(() 
                => new AbstractLoggingRepository<Comment>(contextMock.Object, logRepository.Object));
            var genreRepository = new Lazy<IRepository<Genre>>(() 
                => new AbstractLoggingRepository<Genre>(contextMock.Object, logRepository.Object));
            var platformTypeRepository = new Lazy<IRepository<PlatformType>>(() 
                => new AbstractLoggingRepository<PlatformType>(contextMock.Object, logRepository.Object));
            var publisherRepository = new Lazy<IRepository<Publisher>>(() 
                => new AbstractLoggingRepository<Publisher>(contextMock.Object, logRepository.Object));
            var orderDetailsRepository = new Lazy<IRepository<OrderDetails>>(() 
                => new AbstractLoggingRepository<OrderDetails>(contextMock.Object, logRepository.Object));
            var orderRepository = new Lazy<IRepository<Order>>(() 
                => new AbstractLoggingRepository<Order>(contextMock.Object, logRepository.Object));
            var roleRepository = new Lazy<IRepository<Role>>(() 
                => new AbstractLoggingRepository<Role>(contextMock.Object, logRepository.Object));
            var userRepository = new Lazy<IUserRepository>(() 
                => new UserRepository(contextMock.Object, logRepository.Object));
            _unitOfWork = new UnitOfWork(
                contextMock.Object,
                gameRepository,
                commentRepository,
                genreRepository,
                platformTypeRepository,
                publisherRepository,
                orderRepository,
                orderDetailsRepository,
                roleRepository,
                userRepository);
        }

        [Fact]
        public void GetGameRepository_SomeData_ReturnsObject()
        {
            var result = _unitOfWork.Games;

            Assert.NotNull(result);
        }

        [Fact]
        public void GetCommentRepository_SomeData_ReturnsObject()
        {
            var result = _unitOfWork.Comments;

            Assert.NotNull(result);
        }

        [Fact]
        public void GetGenreRepository_SomeData_ReturnsObject()
        {
            var result = _unitOfWork.Genres;

            Assert.NotNull(result);
        }

        [Fact]
        public void GetOrderDetailsRepository_SomeData_ReturnsObject()
        {
            var result = _unitOfWork.OrderDetails;

            Assert.NotNull(result);
        }

        [Fact]
        public void GetPlatformTypeRepository_SomeData_ReturnsObject()
        {
            var result = _unitOfWork.PlatformTypes;

            Assert.NotNull(result);
        }

        [Fact]
        public void GetPublisherRepository_SomeData_ReturnsObject()
        {
            var result = _unitOfWork.Publishers;

            Assert.NotNull(result);
        }

        [Fact]
        public void GetOrderRepository_SomeData_ReturnsObject()
        {
            var result = _unitOfWork.Orders;

            Assert.NotNull(result);
        }

        [Fact]
        public void GetUserRepository_SomeData_ReturnsObject()
        {
            var result = _unitOfWork.Users;

            Assert.NotNull(result);
        }

        [Fact]
        public void GetRoleRepository_SomeData_ReturnsObject()
        {
            var result = _unitOfWork.Roles;

            Assert.NotNull(result);
        }
    }
}