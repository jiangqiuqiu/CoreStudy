using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IdentitySample.Data;

namespace IdentitySample
{
    /********************************
     * 
     * 在.NET Core中DI的核心分为两个组件：IServiceCollection和 IServiceProvider
     * 
     * IServiceCollection 负责注册:IServiceCollection对象是一个存放服务注册信息的集合
     * 
     * IServiceProvider 负责提供实例
     * 
     * DI框架将服务注册存储在一个通过IServiceCollection接口表示的集合之中。
     * 一个IServiceCollection对象本质上就是一个元素类型为ServiceDescriptor的列表。
     * 我们在应用启动的时候所做的服务注册就是创建出现相应的ServiceDescriptor对象并将其添加到指定IServiceCollection集合对象中的过程。
     * 考虑到服务注册是一个高频调用的操作，所以DI框架为IServiceCollection接口定义了一系列扩展方法完成服务注册的工作
     * 
     * 包含服务注册信息的IServiceCollection对象最终被用来创建作为DI容器的IServiceProvider对象。
     * 当需要消费某个服务实例的时候，我们只需要指定服务类型调用IServiceProvider的GetService方法，
     * IServiceProvider就会根据对应的服务注册提供所需的服务实例。
     * 
     * 
     * GetService和GetRequiredService的区别 ☆
     * 除了定义在IServiceProvider的这个GetService方法，D
     * I框架为了该接口定了如下这些扩展方法。
     * GetService<T>方法会泛型参数的形式指定了服务类型，
     * 返回的服务实例也会作对应的类型转换。
     * 如果指定服务类型的服务注册不存在，GetService方法会返回Null，
     * 如果调用GetRequiredService或者GetRequiredService<T>方法则会抛出一个InvalidOperationException类型的异常。
     * 如果所需的服务实例是必需的，我们一般会调用者两个扩展方法。
     * 
     * Scope指的是由IServiceScope接口表示的“服务范围”，
     * 该范围由IServiceScopeFactory接口表示的“服务范围工厂”来创建。
     * IServiceProvider的扩展方法CreateScope正是利用提供的IServiceScopeFactory服务实例来创建作为服务范围的IServiceScope对象。
     * *******************************/
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().MigrateDbContext<ApplicationDbContext>((context,services)=> {
                new ApplicationDbContextSeed().SeedAsync(context,services).Wait();
            }).Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
