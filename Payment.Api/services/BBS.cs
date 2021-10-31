using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Payment.Api.Dtos;
using Payment.Api.Models;

namespace Payment.Api.Services
{

    class Account
    {
        public string AccountNumber { get; set; }
        public Double Balance { get; set; }
    }
    public class BBS : IPayment
    {

        List<Account> AccountNumbers = new List<Account> { new Account { AccountNumber = "00900000", Balance = 1_000_000 } };

        Account GetAccountByNumber(string accountNumber)
        {
            return AccountNumbers.FirstOrDefault(acct => acct.AccountNumber == accountNumber);
        }
        public BalanceResponse GetAccountBalance(BalanceRequest balanceRequest)
        {
            BalanceResponse balanceRead = new BalanceResponse
            {
                AccountNumber = balanceRequest.AccountNumber,
                Balance = 0,
                CorporateCode = balanceRequest.CorporateCode,
            };
            Account account = GetAccountByNumber(balanceRequest.AccountNumber);
            if (account == null)
            {
                balanceRead.StatusCode = 404;
                balanceRead.StatusDescription = "Account does not exist";
                return balanceRead;
            }

            balanceRead.Balance = account.Balance;
            balanceRead.StatusCode = 200;
            balanceRead.StatusDescription = "Balance successfully retrieved";
            return balanceRead;
        }

        public void AddPayment(AddPaymentRequest addPaymentRequest)
        {
            Account account = GetAccountByNumber(addPaymentRequest.SourceAccount);

            throw new System.NotImplementedException();
        }
    }
}
