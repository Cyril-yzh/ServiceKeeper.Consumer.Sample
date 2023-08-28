using CloudPlatformMessageProvider.CloudPlatform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CloudPlatformMessageProvider.DependencyInjection
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddDingClient(this IServiceCollection services)
        {
            services.AddSingleton<DingAccessTokenClient>(sp =>
            {
                var httpClientFactory = sp.GetService<IHttpClientFactory>() ?? throw new Exception("IHttpClientFactory 获取失败,请先 builder.Services.AddHttpClient();");
                var optionLocal = sp.GetRequiredService<IOptions<DingTalkPlatformOptions>>().Value;
                return new DingAccessTokenClient(httpClientFactory, optionLocal);
            });
            services.AddSingleton<DingUserClient>(sp =>
            {
                var httpClientFactory = sp.GetService<IHttpClientFactory>() ?? throw new Exception("IHttpClientFactory 获取失败,请先 builder.Services.AddHttpClient();");
                var optionLocal = sp.GetRequiredService<IOptions<DingTalkPlatformOptions>>().Value;
                return new DingUserClient(httpClientFactory, optionLocal);
            });
            services.AddSingleton<DingDepartmentClient>(sp =>
            {
                var httpClientFactory = sp.GetService<IHttpClientFactory>() ?? throw new Exception("IHttpClientFactory 获取失败,请先 builder.Services.AddHttpClient();");
                var optionLocal = sp.GetRequiredService<IOptions<DingTalkPlatformOptions>>().Value;
                return new DingDepartmentClient(httpClientFactory, optionLocal);
            });
            services.AddSingleton<DingRoleClient>(sp =>
            {
                var httpClientFactory = sp.GetService<IHttpClientFactory>() ?? throw new Exception("IHttpClientFactory 获取失败,请先 builder.Services.AddHttpClient();");
                var optionLocal = sp.GetRequiredService<IOptions<DingTalkPlatformOptions>>().Value;
                return new DingRoleClient(httpClientFactory, optionLocal);
            });
            services.AddSingleton<DingMessageClient>(sp =>
            {
                var httpClientFactory = sp.GetService<IHttpClientFactory>() ?? throw new Exception("IHttpClientFactory 获取失败,请先 builder.Services.AddHttpClient();");
                var optionLocal = sp.GetRequiredService<IOptions<DingTalkPlatformOptions>>().Value;
                return new DingMessageClient(httpClientFactory, optionLocal);
            });

            return services;
        }
    }
}
