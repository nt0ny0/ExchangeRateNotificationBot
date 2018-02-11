using System;
using Autofac;
using Bot;
using Telegram.Bot;
using TelegramProviders;
using TelegramProviders.Domain;

namespace Telegram.Autofac.DI
{
    public class TelegramModule : Module
    {
        private readonly string _telegramToken;

        public TelegramModule(string telegramToken)
        {
            if (string.IsNullOrEmpty(telegramToken))
            {
                throw new ArgumentException($"{nameof(telegramToken)} is null or empty");
            }
            _telegramToken = telegramToken;
        }
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register<ITelegramBotClient>((ctx) => new TelegramBotClient(_telegramToken)).SingleInstance();
            builder.RegisterType<TelegramProvider>()
                .As<IMessengerProvider>()
                .SingleInstance();
            builder.RegisterType<TelegramUpdateReader>()
                .As<ITelegramUpdateReader>()
                .SingleInstance();
            builder.RegisterType<UserMessageRequestParser>()
                .As<IUserMessageRequestParser>()
                .SingleInstance();
            builder.RegisterType<ExchangeRateBot>()
                .As<IMessengerBot>()
                .SingleInstance();
        }
    }
}
