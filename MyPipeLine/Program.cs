using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MyPipeLine
{
    class Program
    {
        public static List<Func<RequestDelegate, RequestDelegate>> _list = new List<Func<RequestDelegate, RequestDelegate>>();


        static void Main(string[] args)
        {
            Use(next => {
                return context =>
                {
                    Console.WriteLine("1");
                    return next.Invoke(context);
                    //return Task.CompletedTask;
                };
            });

            Use(next => {
                return context =>
                {
                    Console.WriteLine("2");
                    return next.Invoke(context);
                };
            });

            RequestDelegate end = (context) =>{
               Console.WriteLine("end...");
               return Task.CompletedTask;
            };

            foreach (var middleware in _list)
            {
                end = middleware.Invoke(end);
            }

            end.Invoke(new Context());

            Console.ReadLine();
        }

        public static void Use(Func<RequestDelegate,RequestDelegate> middleware)
        {
            _list.Add(middleware);
        }
    }
}
