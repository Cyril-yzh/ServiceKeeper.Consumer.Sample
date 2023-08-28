namespace ServiceKeeper.Consumer.Sample.Domain.Entity
{
    /// <summary>
    /// 由于钉钉单聊和群聊接口参数不一
    /// 需要同时维护两种模式
    /// </summary>
    public enum MessageSendMode
    {
        WorkNotice,
        WebHook
    }
}
