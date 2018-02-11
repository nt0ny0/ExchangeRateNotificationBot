using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using  TelegramProviders.Domain;
using Telegram.Autofac.DI;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();
            var token = configuration["token"];
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TelegramModule(token));
            var container = builder.Build();
            var bot = container.Resolve<IMessengerBot>();
            bot.Start().Wait();
        }
    }
}
