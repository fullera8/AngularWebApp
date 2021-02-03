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
using AutoMapper;
using System.Reflection;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            //Add identity service, StoreUser is to store user info specific to the app
            // Identity role is for identity configuration
            services.AddIdentity<StoreUser, IdentityRole>(cfg => 
            {
                cfg.User.RequireUniqueEmail = true;                
            })
                .AddEntityFrameworkStores<DutchContext>();

            services.AddAuthentication()
                .AddCookie() //Add token support for the authentication, does not store auth token directly 
                             //.AddJwtBearer();
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = _config["Tokens:Issuer"],
                        ValidAudience = _config["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                    };
                }); //Auth JSON token

            services.AddTransient<IMailService, NullMailService>();

            //Add automapper service
            services.AddAutoMapper(Assembly.GetExecutingAssembly());//look for profiles for mapping

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

            //Add Identity pipeline, put this before routing and endpoints for security
            app.UseAuthentication();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(cfg =>
            {
                cfg.MapControllerRoute("Default",
                        "{controller}/{action}/{id?}",
                        new { controller = "App", Action = "Index" });
            });

        }
    }
}
