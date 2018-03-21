using Data.Service.Core.Interfaces;
using LVMiniApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace LVMiniApi.Tests.ControllerTests
{
    [TestFixture]
    public class WidgetsControllerTests
    {
        private Mock<ILoanRepository> _loanRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
        }

        [Test]
        public void LoanPortfolioPerformance_WhenCalled_ReturnsOkWithData()
        {
            _loanRepositoryMock.Setup(lr => lr.LoanRequestAmountPerYearInquire())
                .Returns(new Dictionary<string, decimal>
                {
                    {"2016", 1},
                    {"2017", 2},
                    {"2018", 3}
                });

            var controller = new WidgetsController(_loanRepositoryMock.Object);

            var result = controller.LoanPortfolioPerformance() as OkObjectResult;

            result.StatusCode.ShouldBe(200);
            var content = result.Value as Dictionary<string, decimal>;
            content.ContainsKey("2016").ShouldBe(true);
            content.ContainsValue(3).ShouldBe(true);
        }
    }
}
