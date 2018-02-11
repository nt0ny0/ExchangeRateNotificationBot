namespace TelegramProviders.Domain.Models
{
    public enum UserRequestType
    {
        Unknown = 1,
        GetComandList = 2,
        GetUsdToRubExchangeRate = 3,
        GetEuroToRubExchangeRate = 4,
        GetEuroToUsdExchangeRate = 5
    }
}