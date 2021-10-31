using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Payment.Api.Controllers;
using Payment.Api.Dtos;
using Payment.Api.Models;
using Payment.Api.Repository;
using Payment.Api.Services;
using Xunit;

namespace Payment.UnitTests
{
    public class PayControllerTests
    {
        static List<PaymentTransactionsRequestDto> paymentTransactionsRequestDto = new List<PaymentTransactionsRequestDto>{ new PaymentTransactionsRequestDto
        {
            Amount = 100,
            AccountNumber = Repo.users[1].Account.AccountNumber,
            Narration = "Transfer"
        }};
        static AddPaymentRequestDto addPaymentRequestDto = new AddPaymentRequestDto { Amount = 100, SourceAccount = Repo.users[0].Account.AccountNumber, paymentTransactions = paymentTransactionsRequestDto };

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

        [Fact]
        public async Task Pay_Is_Successful()
        {
            var paymentStub = new Mock<IPayment>();
            paymentStub.Setup(x => x.AddPayment(It.IsAny<AddPaymentRequest>())).ReturnsAsync(new AddPaymentResponse { StatusCode = 201 });

            PayController controller = new PayController(paymentStub.Object);

            var result = await controller.Pay(addPaymentRequestDto);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }
        [Fact]
        public async Task Pay_ReturnsNotFound()
        {
            var paymentStub = new Mock<IPayment>();
            paymentStub.Setup(x => x.AddPayment(It.IsAny<AddPaymentRequest>())).ReturnsAsync(new AddPaymentResponse { StatusCode = 404 });

            PayController controller = new PayController(paymentStub.Object);
            var result = await controller.Pay(addPaymentRequestDto);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
            result.Result.As<NotFoundObjectResult>().StatusCode.Should().Be(404);
        }

    }
}
