using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;
using Telegram.Autofac.DI;
using TelegramProviders.Domain;

namespace App
{
    public class Application
    {
        public void Run(string token, CancellationToken cancellationToken)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TelegramModule(token));
            var container = builder.Build();
            var bot = container.Resolve<IMessengerBot>();
            Task.Run(() => bot.Start(), cancellationToken);
        }
    }
}
