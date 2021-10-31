using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payment.Api.Dtos;
using Payment.Api.Models;
using Payment.Api.Repository;

namespace Payment.Api.Services
{

    public class BBS : IPayment
    {
        User GetUserByAccountNumber(string accountNumber)
        {
            return Repo.users.FirstOrDefault(user => user.Account.AccountNumber == accountNumber);
        }
        public Task<BalanceResponse> GetAccountBalance(BalanceRequest balanceRequest)
        {
            BalanceResponse balanceRead = new BalanceResponse
            {
                AccountNumber = balanceRequest.AccountNumber,
                Balance = 0,
                CorporateCode = balanceRequest.CorporateCode,
            };
            Account account = GetUserByAccountNumber(balanceRequest.AccountNumber)?.Account;
            if (account == null)
            {
                balanceRead.StatusCode = 404;
                balanceRead.StatusDescription = "Account does not exist";
                return Task.FromResult(balanceRead);
            }

            balanceRead.Balance = account.Balance;
            balanceRead.StatusCode = 200;
            balanceRead.StatusDescription = "Balance successfully retrieved";
            return Task.FromResult(balanceRead);
        }

        public Task<AddPaymentResponse> AddPayment(AddPaymentRequest addPaymentRequest)
        {
            AddPaymentResponse addPaymentResponse = new AddPaymentResponse
            {
                AccountNo = addPaymentRequest.SourceAccount,
                BatchReference = addPaymentRequest.BatchReference,
                CorporateCode = addPaymentRequest.CorporateCode,
            };

            Account sourceAccount = GetUserByAccountNumber(addPaymentRequest.SourceAccount)?.Account;

            if (sourceAccount == null)
            {
                addPaymentResponse.StatusCode = 404;
                addPaymentResponse.StatusDescription = "Account does not exist";
                return Task.FromResult(addPaymentResponse);
            }
            System.Console.WriteLine(sourceAccount.Balance);
            System.Console.WriteLine(addPaymentRequest.Amount);


            for (int i = 0; i < addPaymentRequest.paymentTransactionLocal.Count; i++)
            {
                var trans = addPaymentRequest.paymentTransactionLocal[i];
                Account acct = GetUserByAccountNumber(trans.AccountNumber)?.Account;
                if (acct == null)
                {
                    addPaymentResponse.StatusCode = 404;
                    addPaymentResponse.StatusDescription = $"Account {trans.AccountNumber} does not exist";
                    return Task.FromResult(addPaymentResponse);
                }



            }

            if (sourceAccount.Balance < addPaymentRequest.paymentTransactionLocal.Aggregate(0.0, (acc, tran) => acc + tran.Amount))
            {
                addPaymentResponse.StatusCode = 400;
                addPaymentResponse.StatusDescription = "Insufficient funds";
                return Task.FromResult(addPaymentResponse);
            }

            for (int i = 0; i < addPaymentRequest.paymentTransactionLocal.Count; i++)
            {
                var trans = addPaymentRequest.paymentTransactionLocal[i];
                Account acct = GetUserByAccountNumber(trans.AccountNumber)?.Account;

                acct.Balance += trans.Amount;
                sourceAccount.Balance -= trans.Amount;
            }

            addPaymentResponse.StatusCode = 201;
            addPaymentResponse.StatusDescription = "Payment successfully added";


            return Task.FromResult(addPaymentResponse);
        }
    }
}
