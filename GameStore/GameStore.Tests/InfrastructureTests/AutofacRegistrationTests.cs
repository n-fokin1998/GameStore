using Autofac;
using GameStore.AutofacRegistrations;
using Xunit;

namespace GameStore.Tests.InfrastructureTests
{
    public class AutofacRegistrationTests
    {
        [Fact]
        public void GlobalRegistration_SomeData_ReturnsContainerBuilder()
        {
            var result = GlobalRegistrations.ConfigureContainer(new ContainerBuilder());

            Assert.NotNull(result);
        }
    }
}