using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace ProgrammersBlog.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.Sources.Clear();//t�m kaynaklar� temizliyoruz
                var env = hostingContext.HostingEnvironment;//Cal��ma ortam�m�z� buluyoruz. development yada cal�yor gibi
                //optional kullana biliriz yada kullanmayabiliriz ,reloadOnChange  json dosyam�z yenilendikce yenilensin diyoruz ,{env.EnvironmentName} cal��ma ortam�m�z�n ad�n� alm�� oluyoruz
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                }).UseNLog();
    }
}
