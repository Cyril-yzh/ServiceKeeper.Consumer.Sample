﻿using ReflectionSerializer;
using System.Text.Json;

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
        /// <summary>
        /// 消息文本
        /// </summary>
        public DingBaseTemplate Template { get; init; }

        public DingMessage(MessageTemplateMode messageTemplateMode, string json)
        {
            MessageTemplateMode = messageTemplateMode;
            Template = CreateTemplate(messageTemplateMode, json);
        }

        private static DingBaseTemplate CreateTemplate(MessageTemplateMode messageTemplateMode, string json)
        {
            DingBaseTemplate? result = messageTemplateMode switch
            {
                MessageTemplateMode.Text => JsonSerializer.Deserialize<TextTemplate>(json) ?? null,
                MessageTemplateMode.Markdown => JsonSerializer.Deserialize<MarkdownTemplate>(json) ?? null,
                MessageTemplateMode.Link => JsonSerializer.Deserialize<LinkTemplate>(json) ?? null,
                MessageTemplateMode.ActionCard => JsonSerializer.Deserialize<ActionCardTemplate>(json) ?? null,
                MessageTemplateMode.FeedCard => JsonSerializer.Deserialize<FeedCardTemplate>(json) ?? null,
                _ => throw new Exception("无效的模板模式"),
            };
            if (result == null) throw new Exception("无法将json反序列化为钉钉消息");
            return result;
        }
    }

    public abstract class DingBaseTemplate { };

    public class TextTemplate : DingBaseTemplate
    {
        public string content = null!;
    }

    public class MarkdownTemplate : DingBaseTemplate
    {
        public string title = null!;
        public string text = null!;
    }

    public class LinkTemplate : DingBaseTemplate
    {
        public string messageUrl = null!;
        public string picUrl = null!;
        public string title = null!;
        public string text = null!;
    }


    public class ActionCardTemplate : DingBaseTemplate
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

    public class FeedCardTemplate : DingBaseTemplate
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
