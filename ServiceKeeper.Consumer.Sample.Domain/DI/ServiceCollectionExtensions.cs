using CloudPlatformMessageProvider.CloudPlatform;
using ServiceKeeper.Consumer.Sample.Domain.DomainEventHandlers;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ServiceKeeper.Core.PendingHandlerMediatREvents;

namespace ServiceKeeper.Consumer.Sample.Domain.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddSingleton<DingTalkMessageClient>(sp =>
            {
                var memoryCache = sp.GetRequiredService<IMemoryCache>();
                var messageClient = sp.GetRequiredService<DingMessageClient>();
                var accessTokenClient = sp.GetRequiredService<DingAccessTokenClient>();
                var userClient = sp.GetRequiredService<DingUserClient>();
                var departmentClient = sp.GetRequiredService<DingDepartmentClient>();
                var roleClient = sp.GetRequiredService<DingRoleClient>();
                return new DingTalkMessageClient(memoryCache, messageClient, accessTokenClient, userClient, departmentClient, roleClient);
            });

            services.AddSingleton<ITaskReceivedEventHandler, TaskReceivedEventHandler>();
            services.AddSingleton<DingTaskDomainService>(sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var messageClient = sp.GetRequiredService<DingTalkMessageClient>();
                return new DingTaskDomainService(mediator, messageClient);
            });
            return services;

        }
    }
}
