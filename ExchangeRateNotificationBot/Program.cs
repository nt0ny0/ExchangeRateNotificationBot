using System;
using System.Threading;
using Autofac;
using Telegram.Autofac.DI;
using Telegram.Bot;
using TelegramProviders.Domain;

namespace ExchangeRateNotificationBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var token = "";
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TelegramModule(token));
            var container = builder.Build();
            var bot = container.Resolve<IMessengerBot>();
            bot.Start().Wait();
        }
    }
}
