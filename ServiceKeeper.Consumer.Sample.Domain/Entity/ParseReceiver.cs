namespace ServiceKeeper.Consumer.Sample.Domain.Entity
{
    /// <summary>
    /// 解析后的钉钉消息发送目标
    /// </summary>
    public class ParseReceiver
    {
        public ParseReceiver(string token, string users, string departments, long agentid, string corpid, string corpsecret)
        {
            Token = token;
            Users = users;
            Departments = departments;
            Agentid = agentid;
            Corpid = corpid;
            Corpsecret = corpsecret;
        }

        public ParseReceiver(string url, long agentid, string corpid, string corpsecret)
        {
            Url = url;
            Agentid = agentid;
            Corpid = corpid;
            Corpsecret = corpsecret;
        }
        public string? Token { get; set; }
        public string? Url { get; set; }
        public long Agentid { get; set; }
        public string Corpid { get; set; }
        public string Corpsecret { get; set; }
        public string? Users { get; set; }
        public string? Departments { get; set; }
    }
}
