using Currency.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Currency.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IConfiguration _configuration;

        public CurrencyController(ICurrencyService currencyService, IConfiguration configuration)
        {
            _currencyService = currencyService;
            _configuration = configuration;
        }

        [HttpGet("rate_usd_kzt")]
        public async Task<IActionResult> GetRateUsdKzt()
        {
            var url = _configuration["CurrencyRateSettings:Url"];
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("URL для получения курса не задан в конфигурации.");
            }

            try
            {
                var rate = await _currencyService.GetUsdToKztRateAsync(url);
                return Ok(rate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

    }
}
