using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CheckmarksService.Models;
using CheckmarksWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CheckmarksService
{
    public class Program
    {
        public static void Main (string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine("A task has been cancelled manually: " + e.ToString());
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    //DI config file and db context 
                    IConfiguration configuration = hostContext.Configuration;
                    ConfigurationOptions config = configuration.GetSection("Constants").Get<ConfigurationOptions>();
                    services.AddSingleton(config);

                    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    services.AddScoped<ApplicationDbContext>(s => new ApplicationDbContext(optionsBuilder.Options));

                    services.AddHostedService<ScheduledService>();

                    // optionsBuilder.UseSqlite(config.ConnString);  //if breaks in db, put this above addScoped

                    string azureConnection = config.AzureConnection;
                    optionsBuilder.UseSqlServer(azureConnection);


                })
                // .ConfigureWebHostDefaults(webBuilder =>
                // {
                //     webBuilder.UseStartup<Startup>();
                //     webBuilder.ConfigureKestrel(o =>
                //     {
                //         o.ConfigureHttpsDefaults(o => 
                //     o.ClientCertificateMode = 
                //         ClientCertificateMode.RequireCertificate);
                //     });
                // })
                ;
    }
}
