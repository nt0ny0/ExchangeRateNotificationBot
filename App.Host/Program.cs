using System;
using System.IO;
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
                Logger.Info("Application stared");
                WaitStopCommand();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Application startind error");
                throw;
            }
        }

       private static void WaitStopCommand()
        {
            bool isContinue = true;
            Console.CancelKeyPress += (sender, args) => isContinue = false;
            Logger.Info("Press CTRL+C to exit");
            while (isContinue) { }
            Logger.Info("Application shutdown");
        }
    }
}
