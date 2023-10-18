using Newtonsoft.Json;
using ServiceKeeper.Core.ReflectionSerializer;

namespace ServiceKeeper.Consumer.Sample.Domain.Entity
{

    [Description("钉钉消息配置项")]
    /// <summary>
    /// 通过MQ传入的钉钉消息
    /// </summary>
    public record DingMessage
    {
        [Label("消息发送样式")]
        public MessageTemplateMode MessageTemplateMode { get; init; }
        [IsMappingType]
        public TextTemplate? TextTemplate { get; init; }
        [IsMappingType]
        public MarkdownTemplate? MarkdownTemplate { get; init; }
        [IsMappingType]
        public LinkTemplate? LinkTemplate { get; init; }
        [IsMappingType]
        public ActionCardTemplate? ActionCardTemplate { get; init; }
        [IsMappingType]
        public FeedCardTemplate? FeedCardTemplate { get; init; }
    }

    public class TextTemplate
    {
        public string content = null!;
    }

    public class MarkdownTemplate
    {
        public string title = null!;
        public string text = null!;
    }

    public class LinkTemplate
    {
        public string messageUrl = null!;
        public string picUrl = null!;
        public string title = null!;
        public string text = null!;
    }


    public class ActionCardTemplate
    {
        public string title = null!;
        public string markdown = null!;
        public int btn_orientation = 0;
        public List<ActionButton> btn_json_list = null!;
    }

    public class ActionButton
    {
        public string title = null!;
        public string action_url = null!;
    }

    public class FeedCardTemplate
    {
        public WebHookLink[] links = null!;
    }

    public class WebHookLink
    {
        public string title = null!;
        public string messageURL = null!;
        public string picURL = null!;
    }
}
