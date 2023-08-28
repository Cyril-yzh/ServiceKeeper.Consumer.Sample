using ReflectionSerializer;
using System.Text.Json;

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
        /// <summary>
        /// 试图解析 TaskExcuteValue 转为 DingTask
        /// 如果失败则返回 null
        /// </summary>
        /// <returns>具体的钉钉消息发送任务</returns>
        public static DingTask? Parse(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default; // 返回默认值，可能是 null 或者其他合适的默认值
            }
            try
            {
                DingTask? result = JsonSerializer.Deserialize<DingTask>(json);
                return result;
            }
            catch
            {
                return default; // 解析失败时返回默认值
            }
        }
    }
}
