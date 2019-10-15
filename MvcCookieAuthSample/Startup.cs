using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

/***********************************
 * Authentication和Authorization的区别
 * Authentication： 你是谁
 * Authorization：能干什么
 * 举个例子：
 * 你要登陆论坛，输入用户名张三，密码1234，密码正确，证明你张三确实是张三，这就是 Authentication；
 * 再一check用户张三是个版主，所以有权限加精删别人帖，这就是 Authorization。
************************************/
namespace MvcCookieAuthSample
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
            //注册Cookie认证服务
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)//注册认证系统
                .AddCookie(options => {//自定义登陆地址，不配置的话则默认为http://localhost:5000/Account/Login
                    options.LoginPath = "/Account/MakeLogin";
                });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();


            //注意app.UseAuthentication方法一定要放在下面的app.UseMvc方法前面，否者后面就算调用HttpContext.SignInAsync进行用户登录后，使用
            //HttpContext.User还是会显示用户没有登录，并且HttpContext.User.Claims读取不到登录用户的任何信息。
            //这说明Asp.Net OWIN框架中MiddleWare的调用顺序会对系统功能产生很大的影响，各个MiddleWare的调用顺序一定不能反
            app.UseAuthentication();//添加身份认证的中间件，在MVC之前
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
