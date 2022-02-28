using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;

namespace API
{
    public class Program
    {
        //public static async Task Main(string[] args)
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Run();
            // using(var scope = host.Services.CreateScope())
            // {
            //     var services = scope.ServiceProvider;
            //     var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            //     try
            //     {
            //          await AppSeed.SeedAsync(loggerFactory);
            //     }
            //     catch (Exception ex)
            //     {
            //         var logger = loggerFactory.CreateLogger<Program>();
            //         logger.LogError(ex, "An error occured during migration/seeding");
            //     }

            // }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}