using System;
using Autofac;
using Bot;
using ExchangeRateService;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using TelegramProviders;
using TelegramProviders.Domain;

namespace Telegram.Autofac.DI
{
    public class TelegramModule : Module
    {
        private readonly IConfiguration _configuration;

        public TelegramModule(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); 
        }
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var telegramToken = _configuration["telegram-token"];
            var exchangeRateServiceBaseUrl = _configuration["exchange-rate-service-url"];
            if (string.IsNullOrEmpty(telegramToken))
            {
                throw new ArgumentException("telegram-token configuration parameter is null or empty");
            }

            builder.Register<ITelegramBotClient>((ctx) => new TelegramBotClient(telegramToken))
                .SingleInstance();
            builder.RegisterType<TelegramProvider>()
                .As<IMessengerProvider>()
                .SingleInstance();
            builder.RegisterType<TelegramUpdateReader>()
                .As<ITelegramUpdateReader>()
                .SingleInstance();
            builder.RegisterType<UserMessageRequestParser>()
                .As<IUserMessageRequestParser>()
                .SingleInstance();
            builder.RegisterType<GoogleExchangeRateServiceResponseParser>()
                .As<IExchangeRateServiceResponseParser>()
                .SingleInstance();
            builder.Register(c =>
                {
                    var parser = c.Resolve<IExchangeRateServiceResponseParser>();
                    return new GoogleExchangeRateService(exchangeRateServiceBaseUrl, parser);
                })
                .As<IExchangeRateService>()
                .SingleInstance();
            builder.RegisterType<ExchangeRateBotHelpCommandTextBuilder>()
                .As<IHelpCommandTextBuilder>()
                .SingleInstance();
            builder.RegisterType<ExchangeRateBot>()
                .As<IMessengerBot>()
                .SingleInstance();
        }
    }
}
