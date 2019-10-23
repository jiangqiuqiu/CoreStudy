using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IdentityServerCenter
{
    /****************************
     * OAuth2.0
     * 步骤：
     * 1、添加NuGet包:IdentityServer4
     * 2、添加Startup配置
     * 3、添加Config.cs配置类
     * 4、更改Identity server4配置 
     * 
     * 注：访问http://localhost:5000/.well-known/openid-configuration
     * identity server 4 的discovery endpoint的地址是
     * http://localhost:5000/.well-known/openid-configuration 里面能找到各种节点和信息
     * 
     * 5、添加客户端配置
     * ***************************/
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:5000")
                .UseStartup<Startup>();
    }
}
