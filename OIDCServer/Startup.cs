using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OIDCServer.Data;
using OIDCServer.Models;
using OIDCServer.Services;
using IdentityServer4.EntityFramework;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace OIDCServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //const string connectionString = @"Server=.;Database=IndentityServer4;UID=sa;PWD=intechhosun;Trusted_Connection=False;";//改用本地电脑的SQL Server了
            const string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=IndentityServer4_Store;Trusted_Connection=True;MultipleActiveResultSets=true";//注意，这里的实例名必须跟SQLSERVER对象资源管理器里面的一模一样
            var migrationAssembly = typeof(Startup).GetType().Assembly.GetName().Name;  //获取程序集名称    注意：下面并没有用这个，因为数据库迁移的时候报错了 

            /**************************
             * 集成EF Core
             * 
             * 不用TestUser了
             * 要引用Nuget包  IdentityServer4.AspNetIdentity
             * 
             * 集成EF Core 配置Client和API
             * 要引用Nuget包 IdentityServer4.EntityFramework
             * 
             * 数据库迁移
             * Add-Migration InitConfiguration -Context ConfigurationDbContext -OutputDir Data\Migrations\IdentityServer\ConfigurationDb
             * Add-Migration InitPersistedGrant -Context PersistedGrantDbContext -OutputDir Data\Migrations\IdentityServer\PersistedGrantDb
             * 
             * Update-Database -Context ConfigurationDbContext 
             * Update-Database -Context PersistedGrantDbContext
             * ***********************/
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            

            services.AddIdentity<ApplicationUser, ApplicationUserRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()//添加开发人员签名凭据
                    //注释掉内存方式
                    //.AddInMemoryApiResources(Config.GetApiResources())//添加内存apiresource
                    //.AddInMemoryClients(Config.GetClients())//添加内存client
                    //.AddInMemoryIdentityResources(Config.GetIdentityResources())//添加系统中的资源
                     // this adds the config data from DB (clients, resources)

                     //改为从EF Core 加载
                    .AddConfigurationStore(options=> {  //Api Client IdentityClaims
                        options.ConfigureDbContext = builder =>
                        {
                            builder.UseSqlServer(connectionString,
                                sql=>sql.MigrationsAssembly("OIDCServer"));
                        };
                    })
                    // this adds the operational data from DB (codes, tokens, consents)
                    .AddOperationalStore(options=> {  //增加了PersistedGrants表
                        options.ConfigureDbContext = builder =>
                        {
                            builder.UseSqlServer(connectionString,
                                sql => sql.MigrationsAssembly("OIDCServer"));
                        };
                    })
                    .AddAspNetIdentity<ApplicationUser>()
                    .Services.AddScoped<IProfileService,ProfileService>();//注册ProfileService
                                                                          //.AddTestUsers(Config.GetTestUsers());//添加测试用户

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });
            #region 注释掉原来的Identity
            //services.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            //});

            //services.AddIdentity<ApplicationUser, ApplicationUserRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options => {
            //        options.LoginPath = "/Account/Login";
            //    });

            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequiredLength = 12;
            //});
            #endregion

            services.AddScoped<ConsentService>();
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

            InitIdentityServerDataBase(app);
            app.UseStaticFiles();
            //app.UseAuthentication();
            app.UseIdentityServer();//添加IdentityServer中间件

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 初始化数据库，将Config里的信息加入到数据表中
        /// </summary>
        /// <param name="app"></param>
        public void InitIdentityServerDataBase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                if (!configurationDbContext.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        configurationDbContext.Clients.Add(client.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }

                if (!configurationDbContext.ApiResources.Any())
                {
                    foreach (var api in Config.GetApiResources())
                    {
                        configurationDbContext.ApiResources.Add(api.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }
                if (!configurationDbContext.IdentityResources.Any())
                {
                    foreach (var identity in Config.GetIdentityResources())
                    {
                        configurationDbContext.IdentityResources.Add(identity.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }
            }
        }
    }
}
