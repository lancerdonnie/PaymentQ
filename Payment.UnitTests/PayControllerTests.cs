using System;
using Payment.Api.Controllers;
using Payment.Api.Dtos;
using Payment.Api.Services;
using Xunit;

namespace Payment.UnitTests
{
    public class PayControllerTests
    {
        [Fact]
        public void Balance_WithNoneExistentAccount_ReturnsNotFound()
        {
            IPayment payment = new BBS();
            PayController controller = new PayController(payment);
            BalanceRequestDto balanceRequestDto = new BalanceRequestDto { AccountNumber = "00000000" };
            // controller.Balance(balanceRequestDto)
        }
    }
}
