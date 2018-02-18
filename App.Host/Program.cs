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
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            try
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                var configuration = configurationBuilder.Build();
                var app = new Application();
                app.Run(configuration);
                _logger.Info("Application stared");
                WaitStopCommand();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Application startind error");
                throw;
            }
        }

       private static void WaitStopCommand()
        {
            bool isContinue = true;
            Console.CancelKeyPress += (sender, args) => isContinue = false;
            _logger.Info("Press CTRL+C to exit");
            while (isContinue) { }
            _logger.Info("Application shutdown");
        }
    }
}
