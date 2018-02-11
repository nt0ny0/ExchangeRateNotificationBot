using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ExchangeRateService
{
    public class GoogleExchangeRateService : IExchangeRateService
    {
        private readonly HtmlDocument _document = new HtmlDocument();
        public async Task<string> GetRate(string from, string to)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress =
                    new Uri("https://finance.google.com");
                var response =
                    await client.GetStringAsync($"/finance/converter?a=1&from={from}&to={to}&meta=ei%3Df4Z_WuDiKIGBsQGX04r4BQ");
                _document.LoadHtml(response);
                string result = _document.GetElementbyId("currency_converter_result").InnerText;
                return result;
            }
        }
    }
}
