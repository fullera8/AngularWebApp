using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace DutchTreat
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Routing/Endpoint service enabled
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //ORDER OF THESE REQUESTS MATTERS
            //picks basic file types (such as index.html) in the root and uses that if generic web server is visited.
            //app.UseDefaultFiles();

            //When something goes wrong, shows a page for developers
            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Add Error Page for users
                app.UseExceptionHandler("/error");
            }
            //Gives the server instruction to serve static files
            app.UseStaticFiles();
            //Adds node modules
            app.UseNodeModules();

            //Turn on MVC
            app.UseRouting();
            app.UseEndpoints(cfg => 
            {
                cfg.MapControllerRoute(
                    "Fallback", //Route Endpoint Name
                    "{controller}/{action}/{id?}", //Pattern to match
                    new 
                        {
                            controller = "App", Action = "Index" 
                        }
                );//Default fallback (homepage)
            });
            //If the page is run as a dev, will rebuild custom
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //will build routing 
            //app.UseRouting();

            //will build endpoints
            //app.UseEndpoints(endpoints =>
            //{
                //app.Run(async context =>//basically turn off URL routing, all pages load single hello world
                //{
                //    await context.Response.WriteAsync("<html><body><h1>Hello World!</h1></body></html>");
                //});
            //});
        }
    }
}
