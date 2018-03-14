namespace ExchangeRateService
{
    public interface IExchangeRateServiceResponseParser
    {
        decimal GetExchangeRateFromResponseString(string response);
    }
}