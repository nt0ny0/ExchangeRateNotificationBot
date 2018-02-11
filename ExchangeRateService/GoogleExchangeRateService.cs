using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ExchangeRateService
{
    public interface IExchangeRateService
    {
        Task<string> GetRateForUsdToRub();
    }

    public class GoogleExchangeRateService : IExchangeRateService
    {
        private readonly HtmlDocument _document = new HtmlDocument();
        public async Task<string> GetRateForUsdToRub()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress =
                    new Uri("https://finance.google.com");
                var response =
                    await client.GetStringAsync("/finance/converter?a=1&from=USD&to=RUB&meta=ei%3Df4Z_WuDiKIGBsQGX04r4BQ");
                _document.LoadHtml(response);
                string result = _document.GetElementbyId("currency_converter_result").InnerText;
                return result;
            }
        }
    }
}
