using ReflectionSerializer;

namespace ServiceKeeper.Consumer.Sample.Domain.Entity
{
    /// <summary>
    /// 钉钉支持的消息发送样式
    /// </summary>
    public enum MessageTemplateMode
    {
        [EnumTypeMapping(typeof(TextTemplate))]
        Text,
        //[EnumTypeMapping(typeof(MarkdownTemplate))]
        Markdown,
        //[EnumTypeMapping(typeof(LinkTemplate))]
        Link,
        //[EnumTypeMapping(typeof(ActionCardTemplate))]
        ActionCard,
        [EnumTypeMapping(typeof(FeedCardTemplate))]
        FeedCard
    }
}
