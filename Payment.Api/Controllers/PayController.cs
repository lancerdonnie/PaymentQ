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
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IPayment _payment;

        public PayController(IPayment payment)
        {
            _payment = payment;
        }

        // [HttpPost("deposit")]
        // public IEnumerable<WeatherForecast> Deposit()
        // {
        //     var rng = new Random();
        //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //     {
        //         Date = DateTime.Now.AddDays(index),
        //         TemperatureC = rng.Next(-20, 55),
        //         Summary = Summaries[rng.Next(Summaries.Length)]
        //     })
        //     .ToArray();
        // }

        [HttpPost]
        public ActionResult<BalanceResponseDto> Balance(BalanceRequestDto balanceRequestDto)
        {
            BalanceRequest balanceRequest = new BalanceRequest { AccountNumber = balanceRequestDto.AccountNumber, CorporateCode = Constants.Payment.CorporateCode };
            BalanceResponse balanceResponse = _payment.GetAccountBalance(balanceRequest);
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
