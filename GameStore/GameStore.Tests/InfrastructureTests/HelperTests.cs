using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BusinessLogicLayer.Domain;
using GameStore.Web.Helpers;
using Moq;
using Xunit;

namespace GameStore.Tests.InfrastructureTests
{
    public class HelperTests
    {
        private readonly HtmlHelper _helper;

        public HelperTests()
        {
            _helper = new HtmlHelper(new ViewContext(), Mock.Of<IViewDataContainer>());
        }

        [Fact]
        public void FilterHelper_SomeData_ReturnsMvcHtmlString()
        {
            var result = _helper.CreateList(
                new List<int>() { 1 }, new List<string>() { "l1" }, "t1", "n1", new[] { 1 });

            Assert.NotNull(result);
        }

        [Fact]
        public void PaginationHelper_OnePage_ReturnsNull()
        {
            var result = _helper.PageLinks(new PageInfo { TotalItems = 1, PageSize = 1 });

            Assert.Null(result);
        }

        [Fact]
        public void PaginationHelper_SomeData_ReturnsNull()
        {
            var result = _helper.PageLinks(new PageInfo { TotalItems = 50, PageSize = 5, PageNumber = 3 });

            Assert.NotNull(result);
        }
    }
}