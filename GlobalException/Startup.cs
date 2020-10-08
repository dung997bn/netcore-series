using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GlobalException
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //default
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}



            //way 1
            // app.UseExceptionHandler("/api/error");


            //way 2
            //app.UseExceptionHandler(new ExceptionHandlerOptions
            //{
            //    ExceptionHandlingPath= "/api/error"
            //});


            //way 3 not recommend
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
               ExceptionHandler = (c) =>
               {
                   var exception = c.Features.Get<IExceptionHandlerFeature>();
                   var statusCode = exception.Error.GetType().Name switch
                   {
                       "ArgumentException" => HttpStatusCode.BadRequest,
                       _ => HttpStatusCode.ServiceUnavailable
                   };

                   c.Response.StatusCode = (int)statusCode;
                   var content = Encoding.UTF8.GetBytes($"Error [{exception.Error.Message}]");
                   c.Response.Body.WriteAsync(content, 0, content.Length);

                   return Task.CompletedTask;
               }
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
