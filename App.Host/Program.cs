using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using App;

namespace App.Host
{
    class Program
    {
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
                Console.WriteLine("Application stared");
                WaitStopCommand();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

       private static void WaitStopCommand()
        {
            bool isContinue = true;
            Console.CancelKeyPress += (sender, args) => isContinue = false;
            Console.WriteLine("Press CTRL+C to exit");
            while (isContinue) { }
            Console.WriteLine("Application shutdown");
        }
    }
}
