using System.Threading.Tasks;

namespace ExchangeRateService
{
    public interface IExchangeRateService
    {
        Task<decimal> GetRate(string from, string to);
    }
}