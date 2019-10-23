using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace IdentityServerCenter
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> 
            { 
                new ApiResource("api","My Api")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId="clientid",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    ClientSecrets=new List<Secret>{ new Secret("secret".Sha256()) },
                    AllowedScopes={ "api"}//这里的名称是GetApiResources里面定义的API名称
                }
            };
        }
    }
}
