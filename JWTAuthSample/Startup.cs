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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JWTAuthSample
{

    /********************************
     * 一、什么是JWT
     * Json web token (JWT), 
     * 是为了在网络应用环境间传递声明而执行的一种基于JSON的开放标准（RFC 7519）。
     * 该token被设计为紧凑且安全的，特别适用于分布式站点的单点登录（SSO）场景。
     * JWT的声明一般被用来在身份提供者和服务提供者间传递被认证的用户身份信息，以便于从资源服务器获取资源，
     * 也可以增加一些额外的其它业务逻辑所必须的声明信息，该token也可直接被用于认证，也可被加密。
     * 
     * JWT(json web token)基于开放标准（RFC 7519)，是一种无状态的分布式的身份验证方式，
     * 主要用于在网络应用环境间安全地传递声明。它是基于JSON的，所以它也像json一样可以在.Net、JAVA、JavaScript,、PHP等多种语言使用。
     * 为什么要使用JWT？ 
     * 传统的Web应用一般采用Cookies+Session来进行认证。但对于目前越来越多的App、小程序等应用来说，
     * 它们对应的服务端一般都是RestFul 类型的无状态的API，再采用这样的的认证方式就不是很方便了。
     * 而JWT这种无状态的分布式的身份验证方式恰好符合这样的需求。
     * 
     * 二、JWT的组成
     * JWT是什么样子的呢？它就是下面这样的一段字符串：
     * eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.
     * eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjAwMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiLmnY7lm5siLCJuYmYiOjE1NjU5MjMxMjIsImV4cCI6MTU2NTkyMzI0MiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1NDIxNCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTQyMTUifQ.
     * Mrta7nftmfXeo_igBVd4rl2keMmm0rg0WkqRXoVAeik
     * 它是由三段“乱码”字符串通过两个“.”连接在一起组成。
     * 官网https://jwt.io/提供了它的验证方式.
     * 
     * 它的三个字符串分别对应了Header、Payload和Signature三部分。
     * Header：
     * {
     * "alg": "HS256", 
     * "typ": "JWT"
     * }
     * 标识加密方式为HS256，Token类型为JWT, 这段JSON通过Base64Url编码形成上例的第一个字符串
     * 
     * Payload是JWT用于信息存储部分，其中包含了许多种的声明（claims）。
     * 可以自定义多个声明添加到Payload中，系统也提供了一些默认的类型
     * iss (issuer)：签发人
     * exp (expiration time)：过期时间
     * sub (subject)：主题
     * aud (audience)：受众
     * nbf (Not Before)：生效时间
     * iat (Issued At)：签发时间
     * jti (JWT ID)：编号
     * 这部分通过Base64Url编码生成第二个字符串。
     * 
     * Signature是用于Token的验证。
     * 它的值类似这样的表达式：Signature = HMACSHA256( base64UrlEncode(header) + "." + base64UrlEncode(payload), secret)，
     * 也就是说，它是通过将前两个字符串加密后生成的一个新字符串。
     * 所以只有拥有同样加密密钥的人，才能通过前两个字符串获得同样的字符串，通过这种方式保证了Token的真实性。
     * 
     * 三、认证流程
     * 所需角色：
     * 认证服务器：用于用户的登录验证和Token的发放。
     * 应用服务器：业务数据接口。被保护的API。
     * 客户端：一般为APP、小程序等。
     * 
     * 1、用户首先通过登录，到认证服务器获取一个Token。
     * 2、在访问应用服务器的API的时候，将获取到的Token放置在请求的Header中。
     * 3、应用服务器验证该Token，通过后返回对应的结果。
     * 
     * 说明：这只是示例方案，实际项目中可能有所不同。
     * 1、对于小型项目，可能认证服务和应用服务在一起。
     * 2、对于复杂一些的项目，可能存在多个应用服务，用户获取到的Token可以在多个分布式服务中被认证，这也是JWT的优势之一。
     * 
     * 
     * *******************************/

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
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));//加载配置信息

            var jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings",jwtSettings);//绑定到JwtSettings的实例上去

            services.AddAuthentication(options=>{
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o=> {
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer=jwtSettings.Issuer,
                    ValidAudience=jwtSettings.Audience,
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
