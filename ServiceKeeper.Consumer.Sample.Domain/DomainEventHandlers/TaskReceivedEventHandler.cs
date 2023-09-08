using ServiceKeeper.Core.PendingHandlerMediatREvents;
using Newtonsoft.Json;
using ServiceKeeper.Consumer.Sample.Domain.Entity;

namespace ServiceKeeper.Consumer.Sample.Domain.DomainEventHandlers
{

    /// <summary>
    /// 处理从ServiceKeeper获取到的任务
    /// 用来前端消息推送
    /// </summary>
    public class TaskReceivedEventHandler : ITaskReceivedEventHandler
    {
        private readonly DingTaskDomainService dingTaskDomainService;
        public TaskReceivedEventHandler(DingTaskDomainService dingTaskDomainService)
        {
            this.dingTaskDomainService = dingTaskDomainService;
        }

        public async Task<bool> Handle(TaskReceivedEvent request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine(request.TaskJson);
                DingTask? dingTask = JsonConvert.DeserializeObject<DingTask>(request.TaskJson);
                if (dingTask != null)
                {
                    await dingTaskDomainService.Send(dingTask);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理任务时发生错误:{ex.Message}");
                return false;
            }
        }
    }
}
