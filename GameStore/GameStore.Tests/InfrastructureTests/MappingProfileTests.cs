using GameStore.Web.Infrastructure;
using Xunit;

namespace GameStore.Tests.InfrastructureTests
{
    public class MappingProfileTests
    {
        [Fact]
        public void InitializeAutoMapper_SomeDate_ReturnsMapperConfiguration()
        {
            var result = MappingProfile.InitializeAutoMapper();

            Assert.NotNull(result);
        }
    }
}