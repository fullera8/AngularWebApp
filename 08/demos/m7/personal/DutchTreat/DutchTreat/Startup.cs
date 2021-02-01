using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DutchTreat
{
    public class Startup
    {
        //config constructor variable
        private readonly IConfiguration _config;

        //Constructor
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMailService, NullMailService>();

            //Add the newtonsoft json service
            services.AddControllers()
                .AddNewtonsoftJson();

            // Support for real mail service and json reference loop handling
            services.AddControllersWithViews()
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            
            //Add the appropriate db contexts, note the lamda is required to know the type of db
            services.AddDbContext<DutchContext>(cfg =>
            {
                cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
            });

            //Need the service to recogize the seeder service at runtime
            services.AddTransient<DutchSeeder>();

            //Add Dutch Repository service
            services.AddScoped<IDutchRepository, DutchRepository>();

            //services.

            //Add newer API services, need at least version 2.1
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseNodeModules();

            app.UseRouting();

            app.UseEndpoints(cfg =>
            {
                cfg.MapControllerRoute("Default",
                        "{controller}/{action}/{id?}",
                        new { controller = "App", Action = "Index" });
            });

        }
    }
}
