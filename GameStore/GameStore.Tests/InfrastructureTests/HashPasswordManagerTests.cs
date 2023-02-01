using GameStore.BusinessLogicLayer.Infrastructure;
using Xunit;

namespace GameStore.Tests.InfrastructureTests
{
    public class HashPasswordManagerTests
    {
        private readonly HashPasswordManager _hashPasswordManager;

        public HashPasswordManagerTests()
        {
            _hashPasswordManager = new HashPasswordManager();
        }

        [Fact]
        public void EncryptPassword_SomeData_ReturnsHashString()
        {
            var result = _hashPasswordManager.EncryptPassword("abc");

            Assert.NotNull(result);
        }

        [Fact]
        public void VerifyPassword_EqualPassword_ReturnsTrue()
        {
            var hash = _hashPasswordManager.EncryptPassword("abc");
            var result = _hashPasswordManager.VerifyPassword("abc", hash);

            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_NotEqualPassword_ReturnsFalse()
        {
            var hash = _hashPasswordManager.EncryptPassword("abc");
            var result = _hashPasswordManager.VerifyPassword("123", hash);

            Assert.False(result);
        }
    }
}