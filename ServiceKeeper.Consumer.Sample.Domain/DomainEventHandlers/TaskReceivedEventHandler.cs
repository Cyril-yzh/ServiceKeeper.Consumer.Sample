using ServiceKeeper.Core.MediatR;
using Newtonsoft.Json;
using ServiceKeeper.Core;
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

        public async Task<EventResult> Handle(TaskReceivedEvent request, CancellationToken cancellationToken)
        {
            try
            {
                //Console.WriteLine(request.TaskJson);
                DingTask? dingTask = JsonConvert.DeserializeObject<DingTask>(request.TaskJson);
                if (dingTask != null)
                {
                    await dingTaskDomainService.Send(dingTask);
                    return new EventResult(Code.Success,"钉钉消息发送完成");
                }
                return new EventResult(Code.ParseError, "钉钉消息发送失败");
            }
            catch (Exception ex)
            {
                return new EventResult(Code.Failure, $"钉钉消息发送时发生错误:{ex.Message}");
            }
        }
    }
}
