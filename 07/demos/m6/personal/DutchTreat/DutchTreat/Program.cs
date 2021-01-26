using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DutchTreat
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(SetupConfiguration)
            .UseStartup<Startup>()
            .Build();

        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            //Remove default configuration options
            builder.Sources.Clear();

            //Add config file where we can set our own configuration variables
            //Set optional = false, reload on start = true
            //Chaining from least to most trustworthy, with the last in the chain most trusted for 
            // var conflict
            builder.AddJsonFile("config.json", false, true)
                //.AddXmlFile("config.xml", true) //Demo different config files
                .AddEnvironmentVariables(); //Allows us to add different environment vars
        }
    }
}
