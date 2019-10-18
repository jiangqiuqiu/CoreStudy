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
using System.Security.Claims;

namespace JWTAuthSample
{
    public class MyTokenValidator : ISecurityTokenValidator
    {
        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; }

        public bool CanReadToken(string securityToken)
        {
            return true;
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;

            if (securityToken!="mytokensecret")
            {
                return null;
            }
            /**********************************
             * Claim 是对被验证主体特征的一种表述.
             * 比如：登录用户名是...，email是...，用户Id是...，其中的“登录用户名”，“email”，“用户Id”就是ClaimType
             * 对应现实中的事物，比如驾照，驾照中的“身份证号码：xxx”是一个claim，“姓名：xxx”是另一个claim。
             * 
             * 一组claims构成了一个identity，具有这些claims的identity就是 ClaimsIdentity ，
             * 驾照就是一种ClaimsIdentity，可以把ClaimsIdentity理解为“证件”，驾照是一种证件，护照也是一种证件。\
             * 
             * ClaimsIdentity的持有者就是 ClaimsPrincipal ，一个ClaimsPrincipal可以持有多个ClaimsIdentity，
             * 就比如一个人既持有驾照，又持有护照。
             ***********************************/

            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("name","jiang"));
            identity.AddClaim(new Claim("SuperAdminOnly", "true"));//基于Policy的授权，要在Claims里增加这个Policy
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, "user"));
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
