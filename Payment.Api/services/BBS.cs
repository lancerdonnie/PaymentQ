using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    class User
    {
        public Account Account { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public Tuple<int, string> Bank { get; set; }
    }
    public class BBS : IPayment
    {
        public List<Tuple<int, string>> banks = new List<Tuple<int, string>>{
            new Tuple<int, string>(101, "Zenith"),
            new Tuple<int, string>(102, "Access"),
            new Tuple<int, string>(103, "Gtb"),
        };

        List<User> users = new List<User> {
            new User { Account = new Account { AccountNumber = "0000001", Balance = 1_000_000 }, Name = "Christiano Ronaldo", Email = "ronaldo@email.com", Password = "ronaldo", Phone = "080000001",Bank = new Tuple<int, string>(101, "Zenith") },
            new User { Account = new Account { AccountNumber = "0000002", Balance = 2_000_000 }, Name = "Lionel Messi", Email = "messi@email.com", Password = "messi", Phone = "080000002" ,Bank = new Tuple<int, string>(101, "Zenith")},
            new User { Account = new Account { AccountNumber = "0000003", Balance = 5_000 }, Name = "Romelu Lukaku", Email = "lukaku@email.com", Password = "lukaku", Phone = "080000003" ,Bank = new Tuple<int, string>(102, "Access")},
            new User { Account = new Account { AccountNumber = "0000004", Balance = 0 }, Name = "Bruno Fernandes", Email = "bruno@email.com", Password = "bruno", Phone = "080000004",Bank = new Tuple<int, string>(102, "Access") },
            new User { Account = new Account { AccountNumber = "0000005", Balance = 15_000_000 }, Name = "Mohammed Salah", Email = "salah@email.com", Password = "salah", Phone = "080000005",Bank = new Tuple<int, string>(103, "Gtb") }
        };

        User GetUserByAccountNumber(string accountNumber)
        {
            return users.FirstOrDefault(user => user.Account.AccountNumber == accountNumber);
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

            Account account = GetUserByAccountNumber(addPaymentRequest.SourceAccount)?.Account;

            if (account == null)
            {
                addPaymentResponse.StatusCode = 404;
                addPaymentResponse.StatusDescription = "Account does not exist";
                return Task.FromResult(addPaymentResponse);
            }

            for (int i = 0; i < addPaymentRequest.paymentTransactionLocal.Count; i++)
            {
                var trans = addPaymentRequest.paymentTransactionLocal[i];
                if (banks.FirstOrDefault((bank) => bank.Item1 == trans.DestinationBankCode) == null)
                {
                    addPaymentResponse.StatusCode = 404;
                    addPaymentResponse.StatusDescription = $"Bank does not exist";
                    return Task.FromResult(addPaymentResponse);
                }
                Account acct = GetUserByAccountNumber(trans.AccountNumber)?.Account;
                if (acct == null)
                {
                    addPaymentResponse.StatusCode = 404;
                    addPaymentResponse.StatusDescription = $"Account {trans.AccountNumber} does not exist";
                    return Task.FromResult(addPaymentResponse);
                }
            }

            addPaymentResponse.StatusCode = 201;
            addPaymentResponse.StatusDescription = "Payment successfully added";


            return Task.FromResult(addPaymentResponse);
        }
    }
}
