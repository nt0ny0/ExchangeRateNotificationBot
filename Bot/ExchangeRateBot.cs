using System;
using System.Threading.Tasks;
using ExchangeRateService;
using TelegramProviders.Domain;
using TelegramProviders.Domain.Models;

namespace Bot
{
    public class ExchangeRateBot : IMessengerBot
    {
        private readonly IExchangeRateService _service;
        private readonly IMessengerProvider _provider;
        private static int _offset = 0;

        private readonly string CommandListText = $@"
<b>Список команд</b>:
{UserCommands.UsdToRubGetRate} - текущий курс доллара к рублю
{UserCommands.EuroToRubGetRate} - текущий курс евро к рублю
{UserCommands.EuroToUsdGetRate} - текущий курс доллара к евро";

        public ExchangeRateBot(IMessengerProvider provider, IExchangeRateService exchangeRateService)
        {
            _service = exchangeRateService ?? throw new ArgumentNullException(nameof(exchangeRateService));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task Start()
        {
            while (true)
            {
                try
                {
                    var requestBatch = await _provider.GetUserRequests(_offset);
                    if (requestBatch != null)
                    {
                        _offset = requestBatch.Offset;
                        foreach (var userRequest in requestBatch.Requests)
                        {
                            switch (userRequest.UserRequestType)
                            {
                                case UserRequestType.GetUsdToRubExchangeRate:
                                {
                                    await HandleGetExchangeRateRequest(userRequest.UserId, "USD", "RUB");
                                    break;
                                }
                                case UserRequestType.GetEuroToRubExchangeRate:
                                {
                                    await HandleGetExchangeRateRequest(userRequest.UserId, "EUR", "RUB");
                                    break;
                                }
                                case UserRequestType.GetEuroToUsdExchangeRate:
                                {
                                    await HandleGetExchangeRateRequest(userRequest.UserId, "EUR", "USD");
                                    break;
                                }
                                default:
                                {
                                    await _provider.SendMessage(userRequest.UserId, CommandListText);
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private async Task HandleGetExchangeRateRequest(string userId, string currencyFrom, string currencyTo)
        {
            string rate = null;
            try
            {
                rate = await _service.GetRate(currencyFrom, currencyTo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _provider.SendMessage(userId, "Сервис временно недоступен");
                ;
            }

            string response = $"{rate} {UserCommands.GetHelp}";
            await _provider.SendMessage(userId, response);
        }
    }
}
