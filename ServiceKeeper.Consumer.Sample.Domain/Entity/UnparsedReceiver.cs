using ReflectionSerializer;
namespace ServiceKeeper.Consumer.Sample.Domain.Entity
{
    [Description("钉钉消息接收者配置")]
    /// <summary>
    /// 未解析的钉钉消息发送目标
    /// </summary>
    public class UnparsedReceiver
    {
        [Description("输入钉钉申请的Agentid,用于指定对应企业")]
        public long Agentid { get; set; }
        [Description("输入钉钉申请的Corpid,用于指定对应企业")]
        public string Corpid { get; set; } = string.Empty;
        [Description("输入钉钉申请的Corpsecret,用于指定对应企业")]
        public string Corpsecret { get; set; } = string.Empty;
        [Description("输入钉钉机器人webHook地址,用于指定对应群聊")]
        /// <summary>
        /// 指定钉钉机器人webHook地址
        /// </summary>
        public List<string>? ToUrls { get; set; }
        [Description("输入企业内钉钉角色id,发送至所有该角色的成员")]
        /// <summary>
        /// 发送至指定角色的成员
        /// </summary>
        public List<long>? ToRoleid { get; set; }
        [Description("输入企业内钉钉角色名称,发送至所有该角色的成员")]
        /// <summary>
        /// 发送至指定角色的成员
        /// </summary>
        public List<string>? ToRoleNames { get; set; }
        ///// <summary>
        ///// 发送至指定的成员
        ///// </summary>
        //public List<string>? ToUserNames { get; set; }
        [Description("输入员工注册钉钉手机号,发送至对应手机号的成员")]
        /// <summary>
        /// 发送至指定的用户
        /// </summary>
        public List<string>? ToUserMobiles { get; set; }
        [Description("输入企业内部门Id,发送至对应部门的成员,不支持遍历下属子部门")]
        /// <summary>
        /// 发送至指定部门的成员，不支持遍历下属子部门
        /// </summary>
        public List<long>? ToDepartmentIds { get; set; }
        [Description("输入企业内部门部门,发送至对应部门的成员,不支持遍历下属子部门,由于,会多次调用接口遍历查找部门，请优先使用id发送")]
        /// <summary>
        /// 发送至指定部门的用户,会多次调用遍历查找部门，请优先使用id发送
        /// </summary>
        public List<string>? ToDepartmentNames { get; set; }

        /// <summary>
        /// 检查是否存在WorkNotice类型接收者
        /// </summary>
        /// <returns>true:存在;false:不存在</returns>
        public static bool HasWorkNoticeReceiver(UnparsedReceiver receiver)
        {
            return (receiver.ToRoleid?.Any() ?? false) || (receiver.ToRoleNames?.Any() ?? false) ||
                   (receiver.ToUserMobiles?.Any() ?? false) || (receiver.ToDepartmentIds?.Any() ?? false) || (receiver.ToDepartmentNames?.Any() ?? false);
        }

        /// <summary>
        /// 检查是否存在WebHook类型接收者
        /// </summary>
        /// <returns>true:存在;false:不存在</returns>
        public static bool HasWebHookReceiver(UnparsedReceiver receiver)
        {
            return receiver.ToUrls?.Any() ?? false;
        }

    }
}
