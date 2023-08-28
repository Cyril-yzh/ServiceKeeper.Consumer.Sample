using CloudPlatformMessageProvider.DependencyInjection;
using ServiceKeeper.Consumer.Sample.Domain.DependencyInjection;
using ServiceKeeper.Consumer.Sample.Domain.Entity;
//using Microsoft.Extensions.Caching.StackExchangeRedis;
using ServiceKeeper.Core;
using ServiceKeeper.Core.DependencyInjection;
using StackExchange.Redis;

namespace ServiceKeeper.Consumer.Sample.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Configuration(builder);
            RegistryService(builder);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
            //����תDocker��������+ͳһConfigurator
            builder.Services.Configure<ServiceOptions>(options =>
            {
                options.MQHostName = "vyzh2019";
                options.MQExchangeName = "echangeEventBusDemo1";
                options.MQUserName = "admin";
                options.MQPassword = "Aa111111";
                options.MQQueueName = "MessageQueue1";
                options.ServiceDescription = "������Ϣ������";
            });
            //builder.Services.Configure<RedisCacheOptions>(options =>
            //{
            //    options.Configuration = "192.168.23.4:6379,password=Sivic2812";
            //    options.InstanceName = "yzh_";
            //});
        }

        public static void RegistryService(WebApplicationBuilder builder)
        {
            //��������ע��
            builder.Services.AddDingClient();
            //// ɨ������ִ�еĳ��򼯺��������õĳ���
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("192.168.23.4:6379,password=Sivic2812"));
            builder.Services.AddMemoryCache();

            //serviceKeeperע��

            builder.Services.AddConsumerServiceKeeper(typeof(ClassA), null);

            //builder.Services.AddServiceKeeperUI();
            builder.Services.AddDomain();

        }
        public static void InitializeService(WebApplication app)
        {
            app.UseConsumerServiceKeeper();

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
        public class ClassA
        {
            public string A;
            public ClassB classB;
        }

        public class ClassB 
        {
            public string A;
            public ClassC classC;
        }

        public class ClassC
        {
            public string A;
            public DingTask dingtask;
        }
    }
}