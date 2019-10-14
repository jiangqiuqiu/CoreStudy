using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OptionsBindSample
{
    public class Startup
    {

        public IConfiguration Configuration { get; set; }
        //private readonly IConfiguration _configuration;//我更喜欢用这种方式

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //绑定整个配置到POCO对象上
            //services.Configure<Class>(Configuration);
            //或者 也可以绑定特定节点的配置
            services.Configure<List<Student>>(Configuration.GetSection("Students"));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //IApplicationLifetime在应用程序开始和结束时候做的
            //applicationLifetime.ApplicationStarted.Register(()=> {
            //    Console.WriteLine("Started");
            //});
            //applicationLifetime.ApplicationStopped.Register(() => {
            //    Console.WriteLine("Stopped");
            //});
            //applicationLifetime.ApplicationStopping.Register(() => {
            //    Console.WriteLine("Stopping");
            //});


            //Options示例代码需要用到MVC
            app.UseMvcWithDefaultRoute();

           
            //Bind 的示例代码
            //app.Run(async (context) =>
            //{
            //    var myClass = new Class();
            //    Configuration.Bind(myClass);


            //    await context.Response.WriteAsync($"ClassNo:{myClass.ClassNo}\r\n");
            //    await context.Response.WriteAsync($"ClassDesc:{myClass.ClassDesc}");
            //    await context.Response.WriteAsync($"{myClass.Students.Count} Students");
            //});
        }
    }
}
