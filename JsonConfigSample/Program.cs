using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace JsonConfigSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("class.json",false,true);//第三个参数是热更新，如果json文件有改动就加载最新的

            var configuration = builder.Build();

            //第一种方式:使用Key读取
            Console.WriteLine($"ClassNo:{configuration["ClassNo"]}");
            Console.WriteLine($"ClassDesc:{configuration["ClassDesc"]}");
            Console.WriteLine("Students");

            Console.Write($"name:{configuration["Students:0:name"]}  ");
            Console.WriteLine($"age:{configuration["Students:0:age"]}");

            Console.Write($"name:{configuration["Students:1:name"]}  ");
            Console.WriteLine($"age:{configuration["Students:1:age"]}");

            Console.Write($"name:{configuration["Students:2:name"]}  ");
            Console.WriteLine($"age:{configuration["Students:2:age"]}");

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            //第二种方式:使用GetValue<T>
            //GetValue方法的泛型形式有两个重载，一个是GetValue("key")，
            //另一个可以指定默认值，GetValue("key",defaultValue)。
            //如果key的配置不存在，第一种的结果为default(T)，第二种的结果则为指定的默认值。
            Console.WriteLine($"ClassNo:{configuration.GetValue<string>("ClassNo")}");
            Console.WriteLine($"ClassDesc:{configuration.GetValue<string>("ClassDesc")}");
            Console.WriteLine("Students");

            Console.Write($"name:{configuration.GetValue<string>("Students:0:name")}  ");
            Console.WriteLine($"age:{configuration.GetValue<string>("Students:0:age")}");

            Console.Write($"name:{configuration.GetValue<string>("Students:1:name")}  ");
            Console.WriteLine($"age:{configuration.GetValue<string>("Students:1:age")}");

            Console.Write($"name:{configuration.GetValue<string>("Students:2:name","houhou")}  ");
            Console.WriteLine($"age:{configuration.GetValue<string>("Students:2:age","15")}");

            Console.ReadLine();
        }
    }
}
