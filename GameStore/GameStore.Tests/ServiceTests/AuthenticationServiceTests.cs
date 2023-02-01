using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Infrastructure;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Services.Auth;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using Moq;
using Xunit;

namespace GameStore.Tests.ServiceTests
{
    public class AuthenticationServiceTests
    {
        private readonly AuthenticationService _authenticationService;
        private readonly Mock<IUserRepository> _userRepsoitoryMock;
        private readonly Mock<IHashPasswordManager> _hashPasswordManagerMock;
        private readonly Mock<IMapper> _mapperMock;

        public AuthenticationServiceTests()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _hashPasswordManagerMock = new Mock<IHashPasswordManager>();
            _userRepsoitoryMock = new Mock<IUserRepository>();
            unitOfWorkMock.Setup(u => u.Users).Returns(_userRepsoitoryMock.Object);
            _authenticationService = new AuthenticationService(
                unitOfWorkMock.Object,
                _mapperMock.Object,
                _hashPasswordManagerMock.Object,
                new UserProvider(new UserIndentity(unitOfWorkMock.Object, _mapperMock.Object)));
        }

        [Fact]
        public void Login_SomeData_ReturnsUser()
        {
            _hashPasswordManagerMock.Setup(m => m.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _userRepsoitoryMock.Setup(u => u.GetUser(It.IsAny<string>())).Returns(new User());
            _mapperMock.Setup(u => u.Map<User, UserDTO>(It.IsAny<User>())).Returns(new UserDTO());

            var result = _authenticationService.Login(It.IsAny<string>(), It.IsAny<string>(), true);

            Assert.NotNull(result);
        }

        [Fact]
        public void Logout_SomeData_ReturnsUser()
        {
            _authenticationService.LogOut();

            Assert.True(true);
        }

        [Fact]
        public void CurrentUser_SomeData_ReturnsIPrincipalImplementation()
        {
            var result = _authenticationService.CurrentUser;

            Assert.NotNull(result);
        }
    }
}