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
     * OIDC中的Scope不仅代表API资源，还代表用户ID，姓名或电子邮件地址等身份资源
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
     * Hybrid Flow 和 implicit flow 是OIDC（OpenID Connect，OAuth2里面没有Hybrid Flow）协议中的术语，
     * Implicit Flow是指使用OAuth2的Implicit流程获取Id Token和Access Token；
     * Hybrid Flow是指混合Authorization Code Flow（OAuth授权码流程）和Implici Flow。
     * 
     * OpenID Connect和OAuth 2.0组合的优点在于，您可以使用单一协议和令牌服务进行单一交换。
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
                    /***************************************
                     * 在implicit流程中，所有的Token都通过浏览器传输，
                     * 这对于id token来说是完全不错的。 
                     * 现在我们也想要一个AccessToken。
                     * id token比身份令牌更加敏感，如果不需要，我们不想让它们暴露于“外部”世界。
                     * OpenID Connect包含一个名为“Hybrid Flow”的流程，
                     * 它可以让我们两全其美，id token通过浏览器通道传输，
                     * 因此客户端可以在做更多的工作之前验证它。 
                     * 如果验证成功，客户端会通过后端通道使用Token服务来获取AccessToken。
                     * ****************************************/
                    AllowedGrantTypes=GrantTypes.Hybrid,//OAuth2.0使用Implicit OIDC使用Hybrid
                    RequireConsent=true,//是否需要用户点击确认进行跳转,非常信任的用户可以不需要（比如自己）
                    RedirectUris={"http://localhost:5001/signin-oidc" },//跳转登录到的客户端的地址： 客户端服务器地址+/signin-oidc 这个是规定的
                    PostLogoutRedirectUris={"http://localhost:5001/signout-callback-oidc"},//跳转登出到的客户端的地址  也是规定好的
                    ClientSecrets=new List<Secret>{ new Secret("secret".Sha256()) },
                    AlwaysIncludeUserClaimsInIdToken=true,
                    //AllowOfflineAccess. 
                    //我们还需要获取Refresh Token, 
                    //这就要求我们的网站必须可以"离线"工作, 
                    //这里离线是指用户和网站之间断开了, 并不是指网站离线了.
                    AllowOfflineAccess=true,
                    AllowedScopes={ //可以访问的Resource
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess
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
