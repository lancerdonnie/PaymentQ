using System;

namespace Payment.Api.Models
{

    public class BalanceRequest
    {
        public string CorporateCode { get; set; } = "0001";
        public string AccountNumber { get; set; }

    }
    public class BalanceResponse
    {
        public string CorporateCode { get; set; }
        public string AccountNumber { get; set; }
        public Double Balance { get; set; }
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }

    }
}