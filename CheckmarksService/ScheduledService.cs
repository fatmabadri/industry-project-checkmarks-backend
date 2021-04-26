using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CheckmarksService.Models;
using CheckmarksWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CheckmarksService
{
    //Worker class
    public class ScheduledService : IHostedService, IDisposable
    {

        private Timer Timer; //for precise scheduling. Currently not using this. 
        private readonly ILogger<ScheduledService> Logger;
        private readonly IServiceScopeFactory ServiceScopeFactory;
        private readonly ConfigurationOptions ConfigOptions;

        public ScheduledService(ILogger<ScheduledService> logger, IServiceScopeFactory serviceScopeFactory, ConfigurationOptions config)
        {
            this.Logger = logger;
            this.ServiceScopeFactory = serviceScopeFactory;
            this.ConfigOptions = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Starting Service...");

            //Get the context instance and assign values to Cipo class
            ApplicationDbContext dbContext = ServiceScopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<ApplicationDbContext>();
            Cipo.CipoContext = dbContext;
            Cipo.ConfigurationOptions = ConfigOptions;
            Cipo.Logger = Logger;

            //Continuously performs tasks until automatically cancelled. Right now it will never cancel.
            while (!cancellationToken.IsCancellationRequested)
            {
                await Cipo.GetClasses(ConfigOptions.CipoUserKey);
                await Task.Delay(ConfigOptions.ScheduledIntervalInMinutes * 60000, cancellationToken);  
            }

            await Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Service stopped.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}
