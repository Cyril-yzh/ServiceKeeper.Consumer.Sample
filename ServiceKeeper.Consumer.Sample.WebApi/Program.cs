using CloudPlatformMessageProvider.DependencyInjection;
using ServiceKeeper.Consumer.Sample.Domain.DependencyInjection;
using ServiceKeeper.Consumer.Sample.Domain.Entity;
//using Microsoft.Extensions.Caching.StackExchangeRedis;
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

            Configuration(builder);
            RegistryService(builder, url);

            var app = builder.Build();
            InitializeService(app);
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        public static void Configuration(WebApplicationBuilder builder)
        {
            //后期转Docker环境变量+统一Configurator
            builder.Services.Configure<ServiceOptions>(options =>
            {
                options.MQHostName = "vyzh2019";
                options.MQExchangeName = "echangeEventBusDemo1";
                options.MQUserName = "admin";
                options.MQPassword = "Aa111111";
                options.ServiceDescription = "发送消息至钉钉";
            });
        }
        public static void RegistryService(WebApplicationBuilder builder, string url)
        {
            //基础服务注入
            builder.Services.AddDingClient();
            //// 扫描正在执行的程序集和所有引用的程序集
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("192.168.23.4:6379,password=Sivic2812"));
            builder.Services.AddMemoryCache();

            //serviceKeeper注入
            builder.Services.AddConsumerServiceKeeper(typeof(ClassA), null);

            //builder.Services.AddServiceKeeperUI();
            builder.Services.AddDomain();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(url, "http://127.0.0.1:5500", "null").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });
        }
        public static void InitializeService(WebApplication app)
        {
            app.UseConsumerServiceKeeper();
            //TestBuilder(app);
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.UseServiceKeeperUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        public static void TestBuilder(IApplicationBuilder app)
        {
            _ = app.ApplicationServices.GetRequiredService<ServiceMetadata>();
        }
        public class ClassA
        {
            public string A;
            public ClassB classB;
        }

        public class ClassB
        {
            public string A;
            public DingTask dingtask;
        }
        public class ClassX
        {
            public string A;
            public ClassY<string> ClassY;
        }
        public class ClassY<T> where T : class
        {
            public string A;
            public T classT = null!;
            public DingTask dingTask;
        }
    }
}