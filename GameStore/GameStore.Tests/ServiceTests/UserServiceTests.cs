using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Infrastructure;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Services;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo.Abstract;
using Moq;
using Xunit;

namespace GameStore.Tests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            var hashMock = new Mock<IHashPasswordManager>();
            var roleRepositoryMock = new Mock<IRepository<Role>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mongoUnitOfWorkMock = new Mock<IMongoUnitOfWork>();
            var mongoLoggerMock = new Mock<ILogRepository>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(unitOfWorkMock.Object, _mapperMock.Object, hashMock.Object);
            unitOfWorkMock.Setup(uow => uow.Users).Returns(_userRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Roles).Returns(roleRepositoryMock.Object);
            mongoUnitOfWorkMock.Setup(m => m.Logs).Returns(mongoLoggerMock.Object);
        }

        [Fact]
        public void GetUserList_SomeData_ReturnsUserList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<User>, List<UserDTO>>(It.IsAny<IEnumerable<User>>())).Returns(
                new List<UserDTO>
            {
                new UserDTO() { Id = 1 },
                new UserDTO() { Id = 2 }
            });

            var result = _userService.GetList();

            Assert.Equal(1, result.FirstOrDefault().Id);
        }

        [Fact]
        public void GetUserList_EmptyData_ReturnsEmptyList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<User>, List<UserDTO>>(It.IsAny<IEnumerable<User>>()))
                .Returns(new List<UserDTO>());

            var result = _userService.GetList();

            Assert.Empty(result);
        }

        [Fact]
        public void GetUserById_SomeId_ReturnsUser()
        {
            _mapperMock.Setup(m => m.Map<User, UserDTO>(It.IsAny<User>()))
                .Returns(new UserDTO() { Id = 1 });

            var result = _userService.GetById(It.IsAny<int>());

            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetUserById_BadId_ReturnsNull()
        {
            var result = _userService.GetById(2);

            Assert.Null(result);
        }

        [Fact]
        public void RegisterUser_NullUser_ReturnFalseSucceeded()
        {
            var result = _userService.Register(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void RegisterUser_ExistentUserWithSameEmail_ReturnFalseSucceeded()
        {
            _mapperMock.Setup(m => m.Map<UserDTO, User>(It.IsAny<UserDTO>())).Returns(
                new User { Email = "e1", Name = "n2" });
            _userRepositoryMock.Setup(g => g.GetList()).Returns(new List<User>
            {
                new User { Id = 1, Email = "e1", Name = "n1" }
            }.AsQueryable());

            var result = _userService.Register(new UserDTO { Id = 1, Email = "e1", Roles = new List<RoleDTO>() });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void RegisterUser_ExistentUserWithSameName_ReturnFalseSucceeded()
        {
            _mapperMock.Setup(m => m.Map<UserDTO, User>(It.IsAny<UserDTO>())).Returns(new User() { Name = "n1" });
            _userRepositoryMock.Setup(g => g.GetList()).Returns(new List<User>
            {
                new User { Id = 1, Name = "n1" }
            }.AsQueryable());

            var result = _userService.Register(new UserDTO { Id = 1, Name = "n1", Roles = new List<RoleDTO>() });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void RegisterUser_ValidUser_ReturnTrueSucceeded()
        {
            _mapperMock.Setup(m => m.Map<UserDTO, User>(It.IsAny<UserDTO>())).Returns(new User() { Name = "n1" });

            var result = _userService.Register(new UserDTO { Id = 1, Name = "n1", Roles = new List<RoleDTO>() });

            Assert.True(result.Succeeded);
        }

        [Fact]
        public void DeleteUser_NullUser_ReturnFalseSucceeded()
        {
            var result = _userService.Delete(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteUser_NonExistenUser_ReturnFalseSucceeded()
        {
            _userRepositoryMock.Setup(g => g.GetById(1)).Returns((User)null);

            var result = _userService.Delete(new UserDTO { Id = 1, Email = "e1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteUser_ExistingUser_ReturnTrueSucceeded()
        {
            _userRepositoryMock.Setup(g => g.GetById(1)).Returns(Mock.Of<User>());

            _userService.Delete(new UserDTO { Id = 1, Email = "e1" });

            _userRepositoryMock.Verify(c => c.Update(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void UpdateUser_NullUser_ReturnFalseSucceeded()
        {
            var result = _userService.Update(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateUser_NonExistenUser_ReturnFalseSucceeded()
        {
            _userRepositoryMock.Setup(g => g.GetById(1)).Returns((User)null);

            var result = _userService.Update(new UserDTO { Id = 1, Email = "e1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateUser_ExistentUserWithSameName_ReturnFalseSucceeded()
        {
            _userRepositoryMock.Setup(g => g.GetList()).Returns(new List<User>
            {
                new User { Id = 2, Name = "n1" }
            }.AsQueryable());

            var result = _userService.Update(new UserDTO { Id = 1, Name = "n1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateUser_ValidUser_ReturnTrueSucceeded()
        {
            _userRepositoryMock.Setup(g => g.GetById(1)).Returns(new User { Id = 1 });

            var result = _userService.Update(new UserDTO
            {
                Id = 1,
                Name = "n1",
                Roles = new List<RoleDTO>
                {
                    new RoleDTO { Id = 1 },
                    new RoleDTO { Id = 2 }
                }
            });

            Assert.True(result.Succeeded);
        }
    }
}