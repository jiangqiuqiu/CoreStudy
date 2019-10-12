using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CommandLineSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new Dictionary<string, string>{
                { "name","jiangqiuqiu"},
                {"age","18"}
            };
            //var settings = new Dictionary<string, string>{

            //};

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)//将键值对形式的配置项加载进内存中作为默认选项
                .AddCommandLine(args);//接收键值对形式的配置项参数

            //经过验证，AddCommandLine和AddInMemoryCollection跟顺序有关，最后加入的那个可以覆盖前面的，但是最后的如果是空的，则会使用前面的那个
            //虽然 AddCommandLine 和 AddInMemoryCollection 可以同时调用，但不同的使用次序，效果是不一样的（后一个会覆盖前一个的内容---浅覆盖）
            var configuration = builder.Build();
            Console.WriteLine($"name:{configuration["name"]}");
            Console.WriteLine($"age:{configuration["age"]}");

            Console.ReadLine();
        }
    }
}
