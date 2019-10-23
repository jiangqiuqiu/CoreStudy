using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClientCredentialApi
{
    /****************************
     * 客户端凭证模式的授权模式ClientCredential
     * 
     * 只需要引用 IdentityServer4.AccessTokenValidation
     * 
     * ***************************/
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:5001")
                .UseStartup<Startup>();
    }
}
