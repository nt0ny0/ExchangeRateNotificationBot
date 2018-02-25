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
                app.Run(token);
                Logger.Info("Application started");
                WaitStopCommand();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Application starting error");
                throw;
            }
        }

        private static void WaitStopCommand()
        {
            Logger.Info("Press CTRL+C to exit");
            var stopEvent = new ManualResetEventSlim();
            Console.CancelKeyPress += (sender, args) =>
            {
                stopEvent.Set();
                args.Cancel = true;
            };
            stopEvent.Wait();
            Logger.Info("Application shut down");
        }
    }
}
