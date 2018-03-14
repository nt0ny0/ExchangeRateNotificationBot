using System;
using System.IO;
using System.Reflection;
using System.Threading;
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
                var rootPath = GetRootPath();
                var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(rootPath)
                    .AddJsonFile("appsettings.json");
                var configuration = configurationBuilder.Build();
                var app = new Application();
                app.Run(configuration);
                Logger.Info("Application started");
                WaitStopCommand();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Application starting error");
                throw;
            }
        }

        private static string GetRootPath()
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            return fileInfo.DirectoryName;
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
