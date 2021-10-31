using System.Threading.Tasks;
using Payment.Api.Models;

namespace Payment.Api.Services
{
    public interface IPayment
    {
        Task<BalanceResponse> GetAccountBalance(BalanceRequest balanceDto);
        Task<AddPaymentResponse> AddPayment(AddPaymentRequest addPaymentRequestDto);
    }
}