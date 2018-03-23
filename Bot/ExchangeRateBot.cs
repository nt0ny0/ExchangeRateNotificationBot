using System;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateService;
using NLog;
using TelegramProviders.Domain;
using TelegramProviders.Domain.Models;

namespace Bot
{
    public class ExchangeRateBot : IMessengerBot
    {
        private readonly IHelpCommandTextBuilder _helpCommandTextBuilder;
        private readonly IExchangeRateService _service;
        private readonly IMessengerProvider _provider;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private volatile bool _continueRunning;
        private readonly ManualResetEventSlim _hasBeenStoppedEvent;

        public ExchangeRateBot(IMessengerProvider provider,
            IExchangeRateService exchangeRateService,
            IHelpCommandTextBuilder helpCommandTextBuilder)
        {
            _service = exchangeRateService ?? throw new ArgumentNullException(nameof(exchangeRateService));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _helpCommandTextBuilder = helpCommandTextBuilder ?? throw new ArgumentNullException(nameof(helpCommandTextBuilder));
            _hasBeenStoppedEvent = new ManualResetEventSlim();
        }

        public async Task Start()
        {
            var offset = 0;
            _continueRunning = true;
            while (_continueRunning)
            {
                try
                {
                    var requestBatch = await _provider.GetUserRequests(offset);
                    if (requestBatch != null)
                    {
                        offset = requestBatch.Offset;
                        foreach (var userRequest in requestBatch.Requests)
                        {
                            await HandleUserRequest(userRequest);
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Error while try to handle user request");
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            _hasBeenStoppedEvent.Set();
        }

        public void Stop()
        {
            _continueRunning = false;
            _hasBeenStoppedEvent.Wait();
        }

        private async Task HandleUserRequest(UserRequest userRequest)
        {
            if (userRequest == null)
            {
                throw new ArgumentNullException(nameof(userRequest));
            }
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
                    var helpText = _helpCommandTextBuilder.BuildHelpCommandText(); 
                    await _provider.SendMessage(userRequest.UserId, helpText);
                    break;
                }
            }
        }

        private async Task HandleGetExchangeRateRequest(string userId, string currencyFrom, string currencyTo)
        {
            decimal rate = 0;
            try
            {
                rate = await _service.GetRate(currencyFrom, currencyTo);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error while try to get exchange rate");
                var serviceUnawailableMessage = _helpCommandTextBuilder.BuildErrorMessage(e);
                await _provider.SendMessage(userId, serviceUnawailableMessage);
                throw;
            }
            string response = $"1 {currencyFrom} =  {rate} {currencyTo} {UserCommands.GetHelp}";
            await _provider.SendMessage(userId, response);
        }
    }
}
