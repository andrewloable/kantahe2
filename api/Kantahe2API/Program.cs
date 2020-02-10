using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kantahe2API.Models;
using Kantahe2Library.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kantahe2API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppState.Status = PlayState.Stopped;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls(Constants.APIHostSetting);
                });
    }
}
