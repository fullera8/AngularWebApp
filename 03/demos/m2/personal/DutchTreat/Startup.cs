using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //ORDER OF THESE REQUESTS MATTERS
            //picks basic file types (such as index.html) in the root and uses that if generic web server is visited.
            app.UseDefaultFiles();
            //Gives the server instruction to serve static files
            app.UseStaticFiles();
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
