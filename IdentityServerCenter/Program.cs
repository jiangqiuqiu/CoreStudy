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
     * OAuth2.0  客户端模式（Client Credentials Grant）——授权服务端
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

    /******************************************
     * ClientCredential模式
     * 客户端模式（Client Credentials Grant）指客户端以自己的名义，而不是以用户的名义，
     * 向"服务提供商"进行认证。严格地说，客户端模式并不属于OAuth框架所要解决的问题。
     * 在这种模式中，用户直接向客户端注册，客户端以自己的名义要求"服务提供商"提供服务，
     * 其实不存在授权问题。
     * 
     * 步骤如下:
     * （A）客户端向认证服务器进行身份认证，并要求一个访问令牌。
     * （B）认证服务器确认无误后，向客户端提供访问令牌。
     *********************************************/
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
