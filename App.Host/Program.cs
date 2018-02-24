using System;
using System.IO;
using System.Net.Mime;
using System.Threading;
using Autofac;
using Microsoft.Extensions.Configuration;
using App;
using NLog;

namespace App.Host
{
    class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            try
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                var configuration = configurationBuilder.Build();
                var token = configuration["telegram-token"];
                var app = new Application();
                CancellationTokenSource source = new CancellationTokenSource();
                app.Run(token, source.Token);
                Logger.Info("Application started");
                WaitStopCommand(source);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Application starting error");
                throw;
            }
        }

        private static void WaitStopCommand(CancellationTokenSource source)
        {
            bool isContinue = true;
            Console.CancelKeyPress += (sender, args) =>
            {
                source.Cancel();
                isContinue = false;
            };
            Logger.Info("Press CTRL+C to exit");
            while (isContinue) { }
            Logger.Info("Application shutdown");
        }
    }
}
