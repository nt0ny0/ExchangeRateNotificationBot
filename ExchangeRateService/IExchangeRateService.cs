using System.Threading.Tasks;

namespace ExchangeRateService
{
    public interface IExchangeRateService
    {
        Task<string> GetRateForUsdToRub();
    }
}