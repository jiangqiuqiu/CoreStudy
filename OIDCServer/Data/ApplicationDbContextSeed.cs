using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OIDCServer.Models;
using Microsoft.Extensions.DependencyInjection;

namespace OIDCServer.Data
{
    public class ApplicationDbContextSeed
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationUserRole> _roleManager;

        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
        {
            if (!context.Roles.Any())//如果没有角色，就添加一个角色
            {
                _roleManager = services.GetRequiredService<RoleManager<ApplicationUserRole>>();

                var role = new ApplicationUserRole() { Name = "Administrators", NormalizedName = "Administrators" };
                var result=await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    throw new Exception("初始默认角色失败"+result.Errors.SelectMany(e=>e.Description));
                }
            }

            if (!context.Users.Any())
            {
                _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                
                var defaultUser = new ApplicationUser {
                    UserName="Administrator",
                    Email ="jessetalk@163.com",
                    NormalizedUserName ="admin",
                    SecurityStamp="admin",
                    Avatar= "http://i2.sinaimg.cn/IT/2011/0920/U74P2DT20110920090946.jpg"
                };
                
                var result = await _userManager.CreateAsync(defaultUser, "123456");
                await _userManager.AddToRoleAsync(defaultUser, "Administrators");//添加角色


                if (!result.Succeeded)
                {
                    throw new Exception("初始默认用户失败");
                }
            }
        }
    }
}
