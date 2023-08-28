using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKeeper.Consumer.Sample.Domain
{
    public class DingCallbackOptions
    {
        //钉钉类型:应用 钉钉应用名称:通知消息
        public long Agentid { get; set; }
        public string Corpid { get; set; } = string.Empty;
        public string Corpsecret { get; set; } = string.Empty;
        /// <summary>
        /// 在回调中显示的跳转url
        /// </summary>
        public string ConfigurationUrl { get; set; } = string.Empty;
        public string CallbackTilte { get; set; } = string.Empty;
    }
}
