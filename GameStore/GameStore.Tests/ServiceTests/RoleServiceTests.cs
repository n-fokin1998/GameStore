using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Services;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo.Abstract;
using Moq;
using Xunit;

namespace GameStore.Tests.ServiceTests
{
    public class RoleServiceTests
    {
        private readonly Mock<IRepository<Role>> _roleRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RoleService _roleService;

        public RoleServiceTests()
        {
            _roleRepositoryMock = new Mock<IRepository<Role>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mongoUnitOfWorkMock = new Mock<IMongoUnitOfWork>();
            var mongoLoggerMock = new Mock<ILogRepository>();
            _mapperMock = new Mock<IMapper>();
            _roleService = new RoleService(unitOfWorkMock.Object, _mapperMock.Object);
            unitOfWorkMock.Setup(uow => uow.Roles).Returns(_roleRepositoryMock.Object);
            mongoUnitOfWorkMock.Setup(m => m.Logs).Returns(mongoLoggerMock.Object);
        }

        [Fact]
        public void GetRoleList_SomeData_ReturnsRoleList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Role>, List<RoleDTO>>(It.IsAny<IEnumerable<Role>>()))
                .Returns(new List<RoleDTO>
            {
                new RoleDTO() { Id = 1 },
                new RoleDTO() { Id = 2 }
            });

            var result = _roleService.GetList();

            Assert.Equal(1, result.FirstOrDefault().Id);
        }

        [Fact]
        public void GetRoleList_EmptyData_ReturnsEmptyList()
        {
            _mapperMock.Setup(m => m.Map<IEnumerable<Role>, List<RoleDTO>>(It.IsAny<IEnumerable<Role>>()))
                .Returns(new List<RoleDTO>());

            var result = _roleService.GetList();

            Assert.Empty(result);
        }

        [Fact]
        public void GetRoleById_SomeId_ReturnsRole()
        {
            _mapperMock.Setup(m => m.Map<Role, RoleDTO>(It.IsAny<Role>()))
                .Returns(new RoleDTO() { Id = 1 });

            var result = _roleService.GetById(It.IsAny<int>());

            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetRoleById_BadId_ReturnsNull()
        {
            var result = _roleService.GetById(2);

            Assert.Null(result);
        }

        [Fact]
        public void AddRole_NullRole_ReturnFalseSucceeded()
        {
            var result = _roleService.Add(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddRole_ExistentRoleWithSameName_ReturnFalseSucceeded()
        {
            _mapperMock.Setup(m => m.Map<RoleDTO, Role>(It.IsAny<RoleDTO>())).Returns(new Role() { Name = "n1" });
            _roleRepositoryMock.Setup(g => g.GetList()).Returns(new List<Role>
            {
                new Role { Id = 1, Name = "n1" }
            }.AsQueryable());

            var result = _roleService.Add(new RoleDTO { Id = 1, Name = "n1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddRole_ValidData_ReturnTrueSucceeded()
        {
            _mapperMock.Setup(m => m.Map<RoleDTO, Role>(It.IsAny<RoleDTO>())).Returns(new Role() { Name = "n1" });

            var result = _roleService.Add(new RoleDTO { Id = 1, Name = "n1" });

            Assert.True(result.Succeeded);
        }

        [Fact]
        public void DeleteRole_NullRole_ReturnFalseSucceeded()
        {
            var result = _roleService.Delete(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteRole_NonExistenRole_ReturnFalseSucceeded()
        {
            _roleRepositoryMock.Setup(g => g.GetItem(1)).Returns((Role)null);

            var result = _roleService.Delete(new RoleDTO { Id = 1, Name = "n1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteRole_ExistingRole_ReturnTrueSucceeded()
        {
            _roleRepositoryMock.Setup(g => g.GetItem(1)).Returns(Mock.Of<Role>());

            _roleService.Delete(new RoleDTO { Id = 1, Name = "n1" });

            _roleRepositoryMock.Verify(c => c.Update(It.IsAny<Role>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void UpdateRole_NullRole_ReturnFalseSucceeded()
        {
            var result = _roleService.Update(null);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateRole_NonExistenRole_ReturnFalseSucceeded()
        {
            _roleRepositoryMock.Setup(g => g.GetItem(1)).Returns((Role)null);

            var result = _roleService.Update(new RoleDTO { Id = 1, Name = "n1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateRole_ExistentRoleWithSameName_ReturnFalseSucceeded()
        {
            _roleRepositoryMock.Setup(g => g.GetList()).Returns(new List<Role>
            {
                new Role { Id = 2, Name = "n1" }
            }.AsQueryable());

            var result = _roleService.Update(new RoleDTO { Id = 1, Name = "n1" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateRole_RightRole_ReturnTrueSucceeded()
        {
            _mapperMock.Setup(m => m.Map<RoleDTO, Role>(It.IsAny<RoleDTO>())).Returns(new Role() { Name = "n1" });
            _roleRepositoryMock.Setup(g => g.GetItem(It.IsAny<int>())).Returns(new Role { Id = 1, Name = "n1" });

            _roleService.Update(new RoleDTO { Id = 1, Name = "n1" });

            _roleRepositoryMock.Verify(c => c.Update(It.IsAny<Role>(), It.IsAny<int>()), Times.Once);
        }
    }
}