using Currency.Interfaces;
using HtmlAgilityPack;
using System.Globalization;

namespace Currency.Services
{
    public class CurrencyService: ICurrencyService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public CurrencyService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<decimal> GetUsdToKztRateAsync(string url)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var xPath = _configuration["CurrencyRateSettings:CurrencyXPath"];
            if (string.IsNullOrEmpty(xPath))
            {
                throw new Exception("XPath не указан в конфигурации.");
            }

            var rateNode = htmlDoc.DocumentNode.SelectSingleNode(xPath);
            if (rateNode == null)
            {
                throw new Exception("Курс валюты не найден по указанному XPath.");
            }

            var rateText = rateNode.InnerText.Trim();
            var culture = new CultureInfo(_configuration["CurrencyRateSettings:Culture"] ?? "en-US");
            if (decimal.TryParse(rateText, NumberStyles.Any, culture, out var rate))
            {
                return rate;
            }
            else
            {
                throw new Exception("Не удалось преобразовать курс в число.");
            }
        }
    }
}
