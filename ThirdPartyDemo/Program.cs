using System;
using System.Net.Http;
using IdentityModel.Client;

namespace ThirdPartyDemo
{
    //ClientCredential模式
    class Program
    {
        static void Main(string[] args)
        {
            //OpenID Connect发现端点的客户端库作为HttpClient的扩展方法提供。
            //该GetDiscoveryDocumentAsync方法返回一个DiscoveryResponse对象，
            //该对象具有发现文档的各种元素的强类型和弱类型访问器。

            //GetDiscoveryDocumentAsync 发现端点
            var client = new HttpClient();
            var diso = client.GetDiscoveryDocumentAsync("http://localhost:5000").Result;
            if (diso.IsError)
            {
                Console.WriteLine(diso.Error);
            }
            else
            {
                //令牌端点
                var tokenResponse = client.RequestTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = diso.TokenEndpoint,
                    GrantType = "client_credentials",
                    ClientId = "clientid",
                    ClientSecret = "secret",
                    Scope = "api"
                }).Result;

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                }
                else
                {
                    Console.WriteLine(tokenResponse.Json);
                    client.SetBearerToken(tokenResponse.AccessToken);
                    var response = client.GetAsync("http://localhost:5001/api/values").Result;

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
