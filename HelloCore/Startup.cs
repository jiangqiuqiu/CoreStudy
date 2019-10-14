using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace HelloCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();//路由依赖注入            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //第一种方式
            app.UseRouter(builder => builder.MapGet("actionfirst", async context => {
                await context.Response.WriteAsync("this is first action");
            }));

            //第二种方式
            RequestDelegate handler = context => context.Response.WriteAsync("this is second action");
            var route = new Route(
                new RouteHandler(handler), 
                "actionsecond",
                app.ApplicationServices.GetRequiredService<IInlineConstraintResolver>());
            app.UseRouter(route);

            //第三种方式：不常用
            app.Map("/task", taskApp => {
                taskApp.Run(async context => {
                    await context.Response.WriteAsync("this is a task");
                });
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
