using CloudPlatformMessageProvider.DependencyInjection;
using ServiceKeeper.Consumer.Sample.Domain.DependencyInjection;
using ServiceKeeper.Consumer.Sample.Domain.Entity;
using ServiceKeeper.Core;
using ServiceKeeper.Core.DependencyInjection;
using StackExchange.Redis;
using System;

namespace ServiceKeeper.Consumer.Sample.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string url = "https://192.168.23.4:17778/";
            builder.WebHost.UseUrls(url);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //����תDocker��������+ͳһConfigurator
            builder.Services.Configure<ServiceKeeperOptions>(options =>
            {
                options.MQHostName = "vyzh2019";
                options.MQExchangeName = "ServiceKeeper";
                options.MQUserName = "admin";
                options.MQPassword = "password";
                options.ServiceDescription = "������Ϣ���ͷ���";
            });
            //��������ע��
            builder.Services.AddDingClient();
            //// ɨ������ִ�еĳ��򼯺��������õĳ���
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("192.168.23.4:6379,password=password"));
            builder.Services.AddMemoryCache();

            //serviceKeeperע��
            builder.Services.AddConsumerServiceKeeper(typeof(DingTask), null);

            //builder.Services.AddServiceKeeperUI();
            builder.Services.AddDomain();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });

            var app = builder.Build();

            app.UseConsumerServiceKeeper();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    

        //public static void TestBuilder(IApplicationBuilder app)
        //{
        //    _ = app.ApplicationServices.GetRequiredService<ServiceMetadata>();
        //}
        //public class ClassA
        //{
        //    public string A;
        //    public ClassB classB;
        //}

        //public class ClassB
        //{
        //    public string A;
        //    public DingTask dingtask;
        //}
        //public class ClassX
        //{
        //    public string A;
        //    public ClassY<string> ClassY;
        //}
        //public class ClassY<T> where T : class
        //{
        //    public string A;
        //    public T classT = null!;
        //    public DingTask dingTask;
        //}
    }
}