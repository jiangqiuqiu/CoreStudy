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
     * ��.NET Core��DI�ĺ��ķ�Ϊ���������IServiceCollection�� IServiceProvider
     * 
     * IServiceCollection ����ע��:IServiceCollection������һ����ŷ���ע����Ϣ�ļ���
     * 
     * IServiceProvider �����ṩʵ��
     * 
     * DI��ܽ�����ע��洢��һ��ͨ��IServiceCollection�ӿڱ�ʾ�ļ���֮�С�
     * һ��IServiceCollection�������Ͼ���һ��Ԫ������ΪServiceDescriptor���б�
     * ������Ӧ��������ʱ�������ķ���ע����Ǵ���������Ӧ��ServiceDescriptor���󲢽�����ӵ�ָ��IServiceCollection���϶����еĹ��̡�
     * ���ǵ�����ע����һ����Ƶ���õĲ���������DI���ΪIServiceCollection�ӿڶ�����һϵ����չ������ɷ���ע��Ĺ���
     * 
     * ��������ע����Ϣ��IServiceCollection�������ձ�����������ΪDI������IServiceProvider����
     * ����Ҫ����ĳ������ʵ����ʱ������ֻ��Ҫָ���������͵���IServiceProvider��GetService������
     * IServiceProvider�ͻ���ݶ�Ӧ�ķ���ע���ṩ����ķ���ʵ����
     * 
     * 
     * GetService��GetRequiredService������ ��
     * ���˶�����IServiceProvider�����GetService������D
     * I���Ϊ�˸ýӿڶ���������Щ��չ������
     * GetService<T>�����᷺�Ͳ�������ʽָ���˷������ͣ�
     * ���صķ���ʵ��Ҳ������Ӧ������ת����
     * ���ָ���������͵ķ���ע�᲻���ڣ�GetService�����᷵��Null��
     * �������GetRequiredService����GetRequiredService<T>��������׳�һ��InvalidOperationException���͵��쳣��
     * �������ķ���ʵ���Ǳ���ģ�����һ��������������չ������
     * 
     * Scopeָ������IServiceScope�ӿڱ�ʾ�ġ�����Χ����
     * �÷�Χ��IServiceScopeFactory�ӿڱ�ʾ�ġ�����Χ��������������
     * IServiceProvider����չ����CreateScope���������ṩ��IServiceScopeFactory����ʵ����������Ϊ����Χ��IServiceScope����
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
