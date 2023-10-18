using ServiceKeeper.Core.ReflectionSerializer;

namespace ServiceKeeper.Consumer.Sample.Domain.Entity
{
    /// <summary>
    /// 钉钉支持的消息发送样式
    /// </summary>
    public enum MessageTemplateMode
    {
        [EnumTypeMapping(typeof(TextTemplate), "TextTemplate")]
        Text,
        [EnumTypeMapping(typeof(MarkdownTemplate), "MarkdownTemplate")]
        Markdown,
        [EnumTypeMapping(typeof(LinkTemplate), "LinkTemplate")]
        Link,
        [EnumTypeMapping(typeof(ActionCardTemplate), "ActionCardTemplate")]
        ActionCard,
        [EnumTypeMapping(typeof(FeedCardTemplate), "FeedCardTemplate")]
        FeedCard
    }
}
