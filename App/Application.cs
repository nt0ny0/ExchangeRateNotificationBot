using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;
using Telegram.Autofac.DI;
using TelegramProviders.Domain;

namespace App
{
    public class Application
    {
        public void Run(IConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TelegramModule(configuration));
            var container = builder.Build();
            var bots = container.Resolve<IEnumerable<IMessengerBot>>();
            var launchers = bots.Select(b => new Action(() => b.Start())).ToArray();
            Parallel.Invoke(launchers);
        }
    }
}
