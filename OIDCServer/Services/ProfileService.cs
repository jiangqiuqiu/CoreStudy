using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using OIDCServer.Models;

namespace OIDCServer.Services
{
    /// <summary>
    /// Profile就是用户资料，ids 4里面定义了一个IProfileService的接口用来获取用户的一些信息
    /// 主要是为当前的认证上下文绑定claims。我们可以实现IProfileService从外部创建claim扩展到ids4里面。
    /// </summary>
    public class ProfileService : IProfileService
    {
        private UserManager<ApplicationUser> _userManager;
      
        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        private async Task<List<Claim>> GetClaimsFromUserAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject,user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName,user.UserName)
            };

            var roles =await _userManager.GetRolesAsync(user);
            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(JwtClaimTypes.Role,role));
            //}
            roles.ToList().ForEach(role=> {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            });


            if (!string.IsNullOrEmpty(user.Avatar))
            {
                claims.Add(new Claim("avatar",user.Avatar));
            }

            return claims;
        }

        /// <summary>
        /// 获取用户Claims
        /// 用户请求userinfo endpoint时会触发该方法
        /// http://localhost:5000/connect/userinfo
        /// GET请求，带Authorization Bearer access_token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            var claims =await GetClaimsFromUserAsync(user);
            context.IssuedClaims = claims;
        }

        /// <summary>
        /// 判断用户是否可用
        /// Identity Server会确定用户是否有效
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = false;

            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            context.IsActive = user != null;//user存在就激活

            /*
             这样还应该判断用户是否已经锁定，那么应该IsActive=false
             */
        }
    }
}
