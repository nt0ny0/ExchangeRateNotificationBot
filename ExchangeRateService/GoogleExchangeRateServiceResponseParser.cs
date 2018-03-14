using System;

namespace ExchangeRateService
{
    public class GoogleExchangeRateServiceResponseParser : IExchangeRateServiceResponseParser
    {
        public decimal GetExchangeRateFromResponseString(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                throw new ArgumentException($"{nameof(response)} is null or empty");
            }
            var repsonseParts = response.Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries);
            if (repsonseParts.Length != 5)
            {
                throw new AggregateException($"Unknown google response format! response: {response}");
            }
            var rate = repsonseParts[3];
            var result = Convert.ToDecimal(rate.Replace('.', ','));
            return result;
        }
    }
}