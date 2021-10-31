using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Payment.Api.Attributes;
using Payment.Api.Models;

namespace Payment.Api.Dtos
{
    public class PaymentTransactionsRequestDto
    {
        [Required]
        public int DestinationBankCode { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public Double Amount { get; set; }
        [Required]
        public string Narration { get; set; }
        // public DateTime ValueDate = new DateTime();
    }

    public class AddPaymentRequestDto
    {
        [Required]
        [EnsureOneItemAttribute(min: 1, ErrorMessage = "Please add a payment transaction")]
        public List<PaymentTransactionsRequestDto> paymentTransactions { get; set; }
        [Required]
        public string Currency = "NGN";
        public string SingleDebitNaration = "";
        public int EnableSingleDebit = 0;
        [Required]
        public DateTime Date = new DateTime();
        // public int TotalTransactions = 1;
        [Required]
        public string SourceAccount { get; set; }
        [Required]
        public Double Amount { get; set; }
        // public int PaymentMethodId = 1;
        // public int PaymentTypeId = 1;
        [Required]
        public string Username = "";
        [Required]
        public string Password = "";

    }

    public class PaymentTransactionsResponseDto
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

    public class AddPaymentResponseDto
    {
        public string BatchReference { get; set; }
        public string AccountNo { get; set; }
        public List<PaymentTransactionsResponseDto> paymentTransactions { get; set; }
    }

}