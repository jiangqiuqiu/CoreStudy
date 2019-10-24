using IdentityModel.Client;
using System;
using System.Net.Http;

namespace PwdThirdPartyDemo
{
    /*****************************
     * OAuth 2.0 资源所有者密码模式允许客户端向令牌服务发送用户名和密码，并获取代表该用户的访问令牌。
     * 
     * 除了通过无法浏览器进行交互的应用程序之外，通常建议不要使用资源所有者密码模式。
     * 一般来说，当您要对用户进行身份验证并请求访问令牌时，
     * 使用其中一个交互式 OpenID Connect 流程通常要好得多。
     * 
     * 当您将令牌发送到身份API终结点时，您会注意到与客户端模式相比有一个小但重要的区别。
     * 访问令牌现在将包含唯一标识用户的sub claim。
     * 通过在调用API之后检查内容变量可以看到这个“sub”，
     * 并且控制器应用程序也会在屏幕上显示该claim。
     * 
     *  {
          "nbf": 1571893325,
          "exp": 1571896925,
          "iss": "http://localhost:5002",
          "aud": [
            "http://localhost:5002/resources",
            "pwdapi"
          ],
          "client_id": "pwdClient",
          "sub": "1",
          "auth_time": 1571893325,
          "idp": "local",
          "scope": [
            "pwdapi"
          ],
          "amr": [
            "pwd"
          ]
        }
     * ***************************/
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient(); // 连接IdentityServer服务端
            var disco = client.GetDiscoveryDocumentAsync("http://localhost:5002").Result;

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }
            else
            {
                //令牌端点  获取ClientCredentials模式的Token
                var tokenResponse = client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "pwdClient",
                    //ClientSecret = "secret",//如果服务端配置RequireClientSecret=false，则无需此密码
                    UserName = "jiang",
                    Password = "123456",
                    Scope = "pwdapi"
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
                    var response = client.GetAsync("http://localhost:5003/api/values").Result;//从资源服务器获取资源（请求访问受保护的API）

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
                        

                Console.WriteLine(tokenResponse.Json);

            }                
        }
    }
}
