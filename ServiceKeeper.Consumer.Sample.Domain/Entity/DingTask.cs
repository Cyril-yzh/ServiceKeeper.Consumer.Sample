using ServiceKeeper.Core.ReflectionSerializer;

namespace ServiceKeeper.Consumer.Sample.Domain.Entity
{
    [Description("根据配置完成钉钉消息发送")]
    /// <summary>
    /// 通过MQ获取的钉钉消息任务
    /// </summary>
    public record DingTask
    {
        public DingMessage SendMessage { get; set; }
        public List<UnparsedReceiver> SendReceivers { get; set; }
        public DingTask(DingMessage sendMessage, List<UnparsedReceiver> sendReceivers)
        {
            SendMessage = sendMessage;
            SendReceivers = sendReceivers;
        }
    }
}

