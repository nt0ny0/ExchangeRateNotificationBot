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

        private const string CommandListText = @"
#### Список команд: ####
/rate текущий курс доллара к рублю";

        public ExchangeRateBot(IMessengerProvider provider, IExchangeRateService exchangeRateService)
        {
            _service = exchangeRateService ?? throw new ArgumentNullException(nameof(exchangeRateService));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task Start()
        {
            while (true)
            {
                var requestBatch = await _provider.GetUserRequests(_offset);
                if (requestBatch != null)
                {
                    _offset = requestBatch.Offset;
                    foreach (var userRequest in requestBatch.Requests)
                    {
                        switch (userRequest.UserRequestType)
                        {
                            case UserRequestType.GetComandList:
                            {
                                await _provider.SendMessage(userRequest.UserId, CommandListText);
                                break;
                            }
                            case UserRequestType.GetExchangeRate:
                            {
                                await _provider.SendMessage(userRequest.UserId, await _service.GetRateForUsdToRub());
                                break;
                            }
                            default:
                            {
                                await _provider.SendMessage(userRequest.UserId, $@"🍊 {CommandListText}");
                                break;
                            }
                        }
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
