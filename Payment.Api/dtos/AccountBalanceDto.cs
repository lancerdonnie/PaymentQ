using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.Api.Dtos
{

    public class BalanceRequestDto
    {
        [Required]
        public string AccountNumber { get; set; }

    }
    public class BalanceResponseDto
    {
        public string AccountNumber { get; set; }
        public Double Balance { get; set; }

    }
}