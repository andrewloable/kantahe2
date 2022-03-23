using ChromeWrapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kantahe2
{
    public class Program
    {
        private static string _host = "http://0.0.0.0:80";
        [STAThread]
        public static void Main(string[] args)
        {
            //var ip = Kantahe2.Data.Kantahe2State.GetLocalIPAddress();
            //var host = $"http://{ip}/qrcode";
            //UI.New(host, 500, 500, true);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(_host);
                });
    }
}
