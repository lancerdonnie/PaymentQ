using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Payment.Api.Dtos;
using Payment.Api.Models;
using Payment.Api.Services;
using Payment.Api.Utilities;

namespace Payment.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[action]")]
    public class PayController : ControllerBase
    {
        private readonly IPayment _payment;

        public PayController(IPayment payment)
        {
            _payment = payment;
        }

        [HttpPost]
        public async Task<ActionResult<AddPaymentResponseDto>> Pay(AddPaymentRequestDto addPaymentRequestDto)
        {
            List<PaymentTransaction> paymentTransactions = addPaymentRequestDto.paymentTransactions.Select<PaymentTransactionsRequestDto, PaymentTransaction>(item =>
            {
                return new PaymentTransaction
                {
                    DestinationBankCode = item.DestinationBankCode,
                    AccountNumber = item.AccountNumber,
                    Amount = item.Amount,
                    Narration = item.Narration,
                    ValueDate = new DateTime(),
                    TransactionReference = Guid.NewGuid().ToString(),
                    // Beneficiary = "TEST USER",
                    // BeneficiaryEmail = "",
                    // BeneficiaryPhone = "",
                };
            }).ToList();

            AddPaymentRequest addPaymentRequest = new AddPaymentRequest
            {
                paymentTransactionLocal = paymentTransactions,
                CorporateCode = Constants.Payment.CorporateCode,
                Currency = addPaymentRequestDto.Currency,
                SingleDebitNaration = addPaymentRequestDto.SingleDebitNaration,
                EnableSingleDebit = addPaymentRequestDto.EnableSingleDebit,
                Date = new DateTime(),
                SourceAccount = addPaymentRequestDto.SourceAccount,
                Amount = addPaymentRequestDto.Amount,
                BatchReference = Guid.NewGuid().ToString(),
                // Username = "XXX",
                // Password = "XXXX",
            };

            AddPaymentResponse addPaymentResponse = await _payment.AddPayment(addPaymentRequest);
            if (addPaymentResponse.StatusCode == 404)
            {
                return NotFound(new Response404 { Title = addPaymentResponse.StatusDescription });
            }
            else if (addPaymentResponse.StatusCode >= 400)
            {
                ModelState.AddModelError("pay", addPaymentResponse.StatusDescription);
                return ValidationProblem(ModelState);
            }

            AddPaymentResponseDto addPaymentResponseDto = new AddPaymentResponseDto
            {
                BatchReference = addPaymentResponse.BatchReference,
                AccountNo = addPaymentResponse.AccountNo,
            };

            return CreatedAtAction("", addPaymentResponseDto);
        }

        [HttpPost]
        public async Task<ActionResult<BalanceResponseDto>> Balance(BalanceRequestDto balanceRequestDto)
        {
            BalanceRequest balanceRequest = new BalanceRequest { AccountNumber = balanceRequestDto.AccountNumber, CorporateCode = Constants.Payment.CorporateCode };
            BalanceResponse balanceResponse = await _payment.GetAccountBalance(balanceRequest);
            if (balanceResponse.StatusCode == 404)
            {
                return NotFound(new Response404 { Title = balanceResponse.StatusDescription });
            }
            else if (balanceResponse.StatusCode >= 400)
            {
                ModelState.AddModelError("balance", balanceResponse.StatusDescription);
                return ValidationProblem(ModelState);
            }
            BalanceResponseDto balanceResponseDto = new BalanceResponseDto { AccountNumber = balanceResponse.AccountNumber, Balance = balanceResponse.Balance };
            return Ok(new Response200 { Data = balanceResponseDto });
        }
    }
}
