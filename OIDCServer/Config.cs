using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIDCServer
{
    /***************************************
     * 资源分为身份资源(Identity resources)和API资源(API resources)
     * 
     * (1)身份资源(Identity resources)
     * 身份资源是用户的用户名，姓名或电子邮件地址等数据。
     * 身份资源具有唯一的名称，可以为其分配任意声明类型。
     * 然后，这些声明将被包含在用户的身份令牌中。
     * 客户端将使用scope参数来请求访问身份资源。
     * 身份资源可以是IdentityServer自带的资源,也可以是自定义的资源
     * 
     * (2)API资源(API resources)
     * API资源是客户端访问API获得的资源
     * 
     * 定义客户端Client
     * 客户端指想要访问资源的Client
     * 
     * 
     * **********************************/
    public class Config
    {
        //所有可以访问的Resource
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("mvc","API Application")
            };
        }

        //客户端
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId="mvc",
                    ClientName="Mvc Client",
                    ClientUri="http://localhost:5001",
                    LogoUri="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1572022302487&di=9ce3478fd00f90b3bd4128e466770654&imgtype=0&src=http%3A%2F%2Fcdn.auth0.com%2Fblog%2Fasp-net-core-tutorial%2Flogo.png",
                    Description="Mvc Client Description",
                    AllowRememberConsent=true,
                    AllowedGrantTypes=GrantTypes.Implicit,//采用隐式模式
                    RequireConsent=true,//是否需要用户点击确认进行跳转,非常信任的用户可以不需要（比如自己）
                    RedirectUris={"http://localhost:5001/signin-oidc" },//跳转登录到的客户端的地址： 客户端服务器地址+/signin-oidc 这个是规定的
                    PostLogoutRedirectUris={"http://localhost:5001/signout-callback-oidc"},//跳转登出到的客户端的地址  也是规定好的
                    ClientSecrets=new List<Secret>{ new Secret("secret".Sha256()) },                    
                    AllowedScopes={ //可以访问的Resource
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    }
                }
            };
        }

        //测试用户
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId="10000",
                    Username="jiang",                    
                    Password="123456"
                }
            };
        }

        //定义系统中的资源
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
    }
}
