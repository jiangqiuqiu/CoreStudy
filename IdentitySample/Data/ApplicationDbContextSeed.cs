using IdentitySample.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace IdentitySample.Data
{
    public class ApplicationDbContextSeed
    {
        private UserManager<ApplicationUser> _userManager;

        public async Task SeedAsync(ApplicationDbContext context,IServiceProvider service)
        {
            if (!context.Users.Any())
            {
                var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
                var defaultUser = new ApplicationUser
                {
                    UserName = "Administrator",
                    Email = "xxxx@163.com",
                    NormalizedUserName = "admin"
                };

               var result = await userManager.CreateAsync(defaultUser,"_aA123456");
                if (!result.Succeeded)
                {
                    throw new  Exception("初始默认用户失败");
                }
            }
        }
    }
}
