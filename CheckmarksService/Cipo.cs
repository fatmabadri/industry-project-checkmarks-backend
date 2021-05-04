using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CheckmarksService.Models;
using CheckmarksService.ViewModels;
using CheckmarksWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CheckmarksService
{
    public static class Cipo
    {
        public static ApplicationDbContext CipoContext;
        public static ConfigurationOptions ConfigurationOptions;
        public static ILogger<ScheduledService> Logger;

        // tQ: replaced hard-coded value with config; passing into both functions
        // private static string userKey = "";

        public static async Task GetClasses(string userKey)
        {
            string url = ConfigurationOptions.CIPOClassUrl;

            using (HttpClient client = new HttpClient())
            {
                string response;

                client.DefaultRequestHeaders.Add("User-Key", userKey);

                while (true)
                {
                    try
                    {
                      response = await client.GetStringAsync(url);
                      Logger.LogInformation("Obtained CIPO Class response! Exiting loop...");
                      break;
                    }
                    catch (Exception e)
                    {
                        Logger.LogWarning("failed to get CIPO Class response, trying again in 5 seconds");
                        await Task.Delay(5000);
                    }
                }

                NICEClassJson responseJson = JsonConvert.DeserializeObject<NICEClassJson>(response);

               // Debug.WriteLine("json response is: " + responseJson.Result[0].Descriptions[0].Name);

                try
                {
                    foreach (NICEClassResultJson item in responseJson.Result)
                    {
                        NICEClass existing = await CipoContext.NICEClasses.FindAsync(item.ClassNumber);

                        if (existing != null)
                        {
                            CipoContext.Entry(existing).CurrentValues.SetValues(new NICEClass(item));
                        }
                        else
                        {
                            await CipoContext.NICEClasses.AddAsync(new NICEClass(item));
                        }

                        try
                        {
                            await CipoContext.SaveChangesAsync();
                        }
                        catch (DbUpdateException e)
                        {
                            Logger.LogError("DbUpdate Exception when updating: " + item);
                        }

                        //Get all the term data for that class.
                        await GetTerms(item.ClassNumber, userKey);
                    }

                    Logger.LogInformation("Successfully updated CIPO Class data in database.");
                }
            
                catch (Exception e)
                {
                    Logger.LogError("Failed to update CIPO class data to database... " + e.ToString());
                }
            }
        }

        public static async Task GetTerms(int classId, string userKey)
        {
            //https://cipo-gsm-ised-isde-apicast-production.api.canada.ca/v1/classes/2/terms?lang=en
            string url = ConfigurationOptions.CIPOTermsBaseUrl + $"/{classId}/terms?lang=en";

            using (HttpClient client = new HttpClient())
            {
                string response;

                client.DefaultRequestHeaders.Add("User-Key", userKey);

                while (true)
                {
                    try
                    {
                        response = await client.GetStringAsync(url);
                        Logger.LogInformation("Obtained CIPO Term response! Exiting loop...");
                        break;
                    }


                    catch (Exception e)
                    {
                        Logger.LogWarning("failed to get CIPO Term response, trying again in 5 seconds...");
                        await Task.Delay(5000);
                    }
                }

                NICETermsJson responseJson = JsonConvert.DeserializeObject<NICETermsJson>(response);

               // Debug.WriteLine("json response is: " + responseJson.Result[0].TermName);

                try
                {
                    foreach (NICETermsResultJson item in responseJson.Result)
                    {
                        NICETerm existing = await CipoContext.NICETerms.FindAsync(item.TermNumber);

                        if (existing != null)
                        {
                            CipoContext.Entry(existing).CurrentValues.SetValues(new NICETerm(item));
                        }
                        else
                        {
                            await CipoContext.NICETerms.AddAsync(new NICETerm(item));
                        }

                        try
                        {
                            await CipoContext.SaveChangesAsync();
                        }
                        catch (DbUpdateException e)
                        {
                            Logger.LogError("DbUpdate Exception when updating: " + item + ".  Error: " + e.ToString());
                        }

                        Logger.LogInformation("Successfully added or updated CIPO term: " + item + " to database!");
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError("Failed to add CIPO Terms to database for class id: " + classId 
                        + ", reason: " + e.ToString());
                }

                Logger.LogInformation("Successfully updated CIPO Term data in database for class id: " + classId);
            }
        }
    }
}
