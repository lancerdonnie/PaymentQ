using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Payment.Api.Controllers;
using Payment.Api.Dtos;
using Payment.Api.Models;
using Payment.Api.Services;
using Payment.Api.Utilities;
using Xunit;

namespace Payment.UnitTests
{
    public class PayControllerTests
    {
        [Fact]
        public async Task Balance_WithNoneExistentAccount_ReturnsNotFound()
        {
            var paymentStub = new Mock<IPayment>();
            paymentStub.Setup(x => x.GetAccountBalance(It.IsAny<BalanceRequest>())).ReturnsAsync(new BalanceResponse { StatusCode = 404 });

            PayController controller = new PayController(paymentStub.Object);
            var result = await controller.Balance(new BalanceRequestDto { AccountNumber = "00000000" });

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Result.As<NotFoundObjectResult>().StatusCode.Should().Be(404);
        }
    }
}
