using System;
using System.Collections.Generic;

namespace Payment.Api.Models
{

    public class PaymentTransaction
    {
        public int DestinationBankCode = 44;
        public string Beneficiary = "TEST USER";
        public string AccountNumber = "00000XXXXX";
        public Double Amount = 1100;
        public string Narration = "ERP TEST2";
        public DateTime ValueDate = new DateTime();
        public string TransactionReference = "TEST1234";
        public string BeneficiaryEmail = "";
        public string BeneficiaryPhone = "";
    }
    public class AddPaymentRequest
    {
        public List<PaymentTransaction> paymentTransactionLocal { get; set; }
        public string CorporateCode = "0001";
        public string Currency = "NGN";
        public string SingleDebitNaration = "";
        public int EnableSingleDebit = 0;
        public DateTime Date = new DateTime();
        public int TotalTransactions = 1;
        public string SourceAccount = "00000XXXXX";
        public Double Amount = 1100;
        public int PaymentMethodId = 1;
        public int PaymentTypeId = 1;
        public string BatchReference = Guid.NewGuid().ToString();
        public string Username = "XXX";
        public string Password = "XXXX";

    }

    public class AddPaymentResponse
    {
        public string CorporateCode { get; set; }
        public string BatchReference { get; set; }
        public string AccountNo { get; set; }
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }

    }

}