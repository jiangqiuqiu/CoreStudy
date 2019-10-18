using System;
using System.Collections.Generic;
using System.Text;
using IdentitySample.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentitySample.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>//这里必须要改成泛型参数
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
