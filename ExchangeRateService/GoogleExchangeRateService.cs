using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ExchangeRateService
{
    public class GoogleExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateServiceResponseParser _rateServiceResponseParser;
        private readonly HtmlDocument _document = new HtmlDocument();
        private readonly Uri _baseAddress;

        public GoogleExchangeRateService(string baseUrl,
            IExchangeRateServiceResponseParser rateServiceResponseParser)
        {
            _rateServiceResponseParser = rateServiceResponseParser ?? throw new ArgumentNullException(nameof(rateServiceResponseParser));
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException($"{nameof(baseUrl)} is null or empty!");
            }
            _baseAddress = new Uri(baseUrl, UriKind.Absolute);
        }

        public async Task<decimal> GetRate(string from, string to)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                var response =
                    await client.GetStringAsync($"/finance/converter?a=1&from={from}&to={to}&meta=ei%3Df4Z_WuDiKIGBsQGX04r4BQ");
                _document.LoadHtml(response);
                string responseText = _document.GetElementbyId("currency_converter_result").InnerText;
                var result = _rateServiceResponseParser.GetExchangeRateFromResponseString(responseText);
                return result;
            }
        }
    }
}
