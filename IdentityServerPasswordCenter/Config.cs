using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerPasswordCenter
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("pwdapi","My PWDAPI")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId="pwdClient",
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,//采用密码模式
                    ClientSecrets=new List<Secret>{ new Secret("secret".Sha256()) },
                    //RequireClientSecret=false,//不验证secret ，一般是信得过的第三方
                    AllowedScopes={ "pwdapi"}//这里的名称是GetApiResources里面定义的API名称
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId="1",
                    Username="jiang",
                    Password="123456"
                }
            };
        }
    }
}
