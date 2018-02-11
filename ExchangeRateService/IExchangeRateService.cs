using System.Threading.Tasks;

namespace ExchangeRateService
{
    public interface IExchangeRateService
    {
        Task<string> GetRate(string from, string to);
    }
}