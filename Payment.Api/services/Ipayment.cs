using Payment.Api.Models;

namespace Payment.Api.Services
{
    public interface IPayment
    {
        BalanceResponse GetAccountBalance(BalanceRequest balanceDto);
        void AddPayment(AddPaymentRequest addPaymentRequestDto);
    }
}