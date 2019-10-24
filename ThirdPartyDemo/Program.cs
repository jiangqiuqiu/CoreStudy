using System;
using System.Net.Http;
using IdentityModel.Client;

namespace ThirdPartyDemo
{
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
    class Program
    {
        static void Main(string[] args)
        {
            //OpenID Connect发现端点的客户端库作为HttpClient的扩展方法提供。
            //该GetDiscoveryDocumentAsync方法返回一个DiscoveryResponse对象，
            //该对象具有发现文档的各种元素的强类型和弱类型访问器。

            //GetDiscoveryDocumentAsync 发现端点
            var client = new HttpClient(); // 连接IdentityServer服务端
            var diso = client.GetDiscoveryDocumentAsync("http://localhost:5000").Result;
            if (diso.IsError)
            {
                Console.WriteLine(diso.Error);
            }
            else
            {
                //令牌端点  获取ClientCredentials模式的Token
                var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = diso.TokenEndpoint,
                    GrantType = "client_credentials",
                    ClientId = "clientid",
                    ClientSecret = "secret",
                    Scope = "api1"
                }).Result;

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                }
                else
                {
                    //访问API
                    Console.WriteLine(tokenResponse.Json);
                    client.SetBearerToken(tokenResponse.AccessToken);
                    var response = client.GetAsync("http://localhost:5001/api/values").Result;//从资源服务器获取资源（请求访问受保护的API）

                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode);
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
