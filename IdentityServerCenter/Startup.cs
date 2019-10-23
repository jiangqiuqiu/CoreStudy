using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityServer4;

namespace IdentityServerCenter
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
            //添加Startup配置
            services.AddIdentityServer()//把IdentityServer注册到容器                                       
                .AddDeveloperSigningCredential() //对于Token签名需要一对公钥和私钥,
                                                 //IdentityServer为开发者提供了一个AddDeveloperSigningCredential()方法，
                                                 //它会帮我们搞定这个事情并且存储到硬盘。
                //基于内存的方式
                .AddInMemoryApiResources(Config.GetApiResources())//哪些api可以使用这个AuthorizationServer
                .AddInMemoryClients(Config.GetClients());//哪些client可以使用这个AuthorizationServer

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //添加Startup配置
            app.UseIdentityServer();
        }
    }
}
