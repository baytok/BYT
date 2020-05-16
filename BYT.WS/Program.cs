using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BYT.WS.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace BYT
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
         .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
         .Build();
        public static void Main(string[] args)
        {
            var appSettingsSection = Configuration.GetSection("SeriLog");
            var appSettings = appSettingsSection.Get<AppSettings>();
            bool IsLoggingEnabled = appSettings.SeriLogEnable;


            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .Filter.ByExcluding(_ => !IsLoggingEnabled) // true olursa loglama kapanýyor
            // Aþaðýdaki bölüm CreateLogger kadar kapatýlýrsa daha detaylý log tutmaya baþlýyor
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq(
                Environment.GetEnvironmentVariable("SEQ_URL") ?? "http://localhost:5341")
            .CreateLogger();

            //Serilog.Debugging.SelfLog.Enable(msg =>
            //{
            //    Debug.Print(msg);
            //    Debugger.Break();
            //});

            try
            {
             //     Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseConfiguration(Configuration);
                    webBuilder.UseSerilog(); //Normal Logger dosayaya yazmak için burasý kapatýlacak;

                });
           
            
    }
}
