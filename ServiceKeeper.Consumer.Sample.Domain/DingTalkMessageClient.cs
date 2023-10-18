using ServiceKeeper.Consumer.Sample.Domain;
using ServiceKeeper.Consumer.Sample.Domain.Entity;
using CloudPlatformMessageProvider;
using System.Net.NetworkInformation;
using CloudPlatformMessageProvider.CloudPlatform;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace ServiceKeeper.Consumer.Sample.Domain
{
    public class DingTalkMessageClient
    {
        private readonly IMemoryCache memoryCache;
        private readonly DingMessageClient messageClient;
        private readonly DingAccessTokenClient accessTokenClient;
        private readonly DingUserClient userClient;
        private readonly DingDepartmentClient departmentClient;
        private readonly DingRoleClient roleClient;

        public DingTalkMessageClient(IMemoryCache memoryCache, DingMessageClient messageClient, DingAccessTokenClient accessTokenClient, DingUserClient userClient, DingDepartmentClient departmentClient, DingRoleClient roleClient)
        {
            this.memoryCache = memoryCache;
            this.messageClient = messageClient;
            this.accessTokenClient = accessTokenClient;
            this.userClient = userClient;
            this.departmentClient = departmentClient;
            this.roleClient = roleClient;
        }
        public static string GetMessage(DingMessage sendMessage, MessageSendMode sendMode, ParseReceiver sendReceiver)
        {
            try
            {
                if (string.IsNullOrEmpty(sendMessage.ToString())) throw new Exception("传入的 message 为空,无法发送");
                string result = "";
                switch (sendMode)
                {
                    case MessageSendMode.WebHook:
                        switch (sendMessage.MessageTemplateMode)
                        {
                            case MessageTemplateMode.Text:
                                result = JsonConvert.SerializeObject(new
                                {
                                    msg = new
                                    {
                                        msgtype = "text",
                                        text = sendMessage.TextTemplate,
                                    },
                                    to_all_user = "false",
                                });
                                break;
                            case MessageTemplateMode.Markdown:
                                result = JsonConvert.SerializeObject(new
                                {
                                    msg = new
                                    {
                                        msgtype = "markdown",
                                        markdown = sendMessage.MarkdownTemplate,
                                    },
                                    to_all_user = "false",
                                });
                                break;
                            case MessageTemplateMode.Link:
                                result = JsonConvert.SerializeObject(new
                                {
                                    msg = new
                                    {
                                        msgtype = "link",
                                        link = sendMessage.LinkTemplate,
                                    },
                                    to_all_user = "false",
                                });
                                break;
                            case MessageTemplateMode.ActionCard:
                                result = JsonConvert.SerializeObject(new
                                {
                                    msg = new
                                    {
                                        msgtype = "actionCard",
                                        actionCard = sendMessage.ActionCardTemplate,
                                    },
                                    to_all_user = "false",
                                });
                                break;
                            case MessageTemplateMode.FeedCard:
                                result = JsonConvert.SerializeObject(new
                                {
                                    msg = new
                                    {
                                        msgtype = "feedCard",
                                        feedCard = sendMessage.FeedCardTemplate,
                                    },
                                    to_all_user = "false",
                                });
                                break;
                            default:
                                break;
                        }
                        break;
                    case MessageSendMode.WorkNotice:
                        switch (sendMessage.MessageTemplateMode)
                        {
                            case MessageTemplateMode.Text:
#if DEBUG
                                sendMessage.TextTemplate!.content = $"{sendMessage.TextTemplate!.content} \r\nDebugTag:{DateTime.Now:hh:mm:ss}";
#endif
                                result = JsonConvert.SerializeObject(new
                                {
                                    msg = new
                                    {
                                        msgtype = "text",
                                        text = sendMessage.TextTemplate,

                                    },
                                    to_all_user = "false",
                                    agent_id = sendReceiver.Agentid,
                                    dept_id_list = string.IsNullOrEmpty(sendReceiver.Departments) ? null : sendReceiver.Departments,
                                    userid_list = string.IsNullOrEmpty(sendReceiver.Users) ? null : sendReceiver.Users
                                });
                                break;
                            case MessageTemplateMode.Markdown:
#if DEBUG
                                sendMessage.MarkdownTemplate!.text = $"{sendMessage.MarkdownTemplate!.text} \r\nDebugTag:{DateTime.Now:hh:mm:ss}";
#endif
                                result = JsonConvert.SerializeObject(new
                                {
                                    msg = new
                                    {
                                        msgtype = "markdown",
                                        markdown = sendMessage.MarkdownTemplate,
                                    },
                                    to_all_user = "false",
                                    agent_id = sendReceiver.Agentid,
                                    dept_id_list = string.IsNullOrEmpty(sendReceiver.Departments) ? null : sendReceiver.Departments,
                                    userid_list = string.IsNullOrEmpty(sendReceiver.Users) ? null : sendReceiver.Users
                                });
                                break;
                            case MessageTemplateMode.Link:
#if DEBUG
                                sendMessage.LinkTemplate!.text = $"{sendMessage.LinkTemplate.text} \r\nDebugTag:{DateTime.Now:hh:mm:ss}";
#endif
                                result = JsonConvert.SerializeObject(new
                                {
                                    msg = new
                                    {
                                        msgtype = "link",
                                        link = sendMessage.LinkTemplate,
                                    },
                                    to_all_user = "false",
                                    agent_id = sendReceiver.Agentid,
                                    dept_id_list = string.IsNullOrEmpty(sendReceiver.Departments) ? null : sendReceiver.Departments,
                                    userid_list = string.IsNullOrEmpty(sendReceiver.Users) ? null : sendReceiver.Users
                                });
                                break;
                            case MessageTemplateMode.ActionCard:
#if DEBUG
                                sendMessage.ActionCardTemplate!.markdown = $"{sendMessage.ActionCardTemplate!.markdown} \r\nDebugTag:{DateTime.Now:hh:mm:ss}";
#endif
                                result = JsonConvert.SerializeObject(new
                                {
                                    msg = new
                                    {
                                        msgtype = "action_card",
                                        action_card = sendMessage.ActionCardTemplate,
                                    },
                                    to_all_user = "false",
                                    agent_id = sendReceiver.Agentid,
                                    dept_id_list = string.IsNullOrEmpty(sendReceiver.Departments) ? null : sendReceiver.Departments,
                                    userid_list = string.IsNullOrEmpty(sendReceiver.Users) ? null : sendReceiver.Users
                                });
                                break;
                            case MessageTemplateMode.FeedCard:
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                if (string.IsNullOrEmpty(result)) throw new Exception("无法解析出Message,无法发送");
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Dictionary<MessageSendMode, ParseReceiver>>> GetReceiver(List<UnparsedReceiver> TaskReceivers)
        {
            try
            {
                //并行获取所有接收者
                List<Dictionary<MessageSendMode, ParseReceiver>> result = new();
                await Task.WhenAll(TaskReceivers.Select(async receiver =>
                {
                    Dictionary<MessageSendMode, ParseReceiver> temp_receiver = new(); //单独创建一个Dictionary避免线程安全问题
                    if (UnparsedReceiver.HasWorkNoticeReceiver(receiver))
                    {
                        string? token = await memoryCache.GetOrCreateAsync($"dingding - {receiver.Corpid} - {receiver.Corpsecret}", async (e) =>
                        {
                            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(100);
                            return await accessTokenClient.GetAccessTokenAsync(receiver.Corpid, receiver.Corpsecret);
                        });
                        //string? token = await accessTokenClient.GetAccessTokenAsync(receiver.Corpid, receiver.Corpsecret);
                        if (token != null)
                        {
                            string users = await DingUserFilter(token, receiver);
                            string departments = await DingDepartmentFilter(token, receiver);
                            if (!string.IsNullOrEmpty(users) || !string.IsNullOrEmpty(departments))
                            {
                                temp_receiver.TryAdd(MessageSendMode.WorkNotice, new ParseReceiver(token, users, departments, receiver.Agentid, receiver.Corpid, receiver.Corpsecret));
                            }
                        }
                    }
                    if (UnparsedReceiver.HasWebHookReceiver(receiver))
                    {
                        foreach (var url in receiver.ToUrls!)
                        {
                            temp_receiver.TryAdd(MessageSendMode.WebHook, new ParseReceiver(url, receiver.Agentid, receiver.Corpid, receiver.Corpsecret));
                        }
                    }
                    if (temp_receiver.Count > 0)
                    {
                        result.Add(temp_receiver);
                    }
                }));
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> Send(MessageSendMode sendMode, ParseReceiver sendReceiver, string json)
        {
            try
            {
                switch (sendMode)
                {
                    case MessageSendMode.WorkNotice:
                        if (string.IsNullOrEmpty(sendReceiver.Token)) return false;
                        return await messageClient.SendMessageNoticeAsync(sendReceiver.Token, json);
                    case MessageSendMode.WebHook:
                        if (string.IsNullOrEmpty(sendReceiver.Url)) return false;
                        return await messageClient.SendMessageRobotAsync(sendReceiver.Url, json);
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 确认消息接收方(员工)
        /// </summary>
        private async Task<string> DingUserFilter(string token, UnparsedReceiver receiver)
        {
            try
            {
                var users = new List<string>();
                if (receiver.ToRoleid?.Any() ?? false) //从roleId获取userid
                {
                    foreach (var roleId in receiver.ToRoleid)
                    {
                        List<string>? temp_userid = await memoryCache.GetOrCreateAsync($"dingding - {roleId}", async (e) =>
                        {
                            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
                            return await roleClient.GetUserIdbyRoleIdAsync(token, roleId);
                        });
                        //List<string>? temp_userid = await roleClient.GetUserIdbyRoleIdAsync(token, roleId);
                        if (temp_userid?.Any() ?? false) users.AddRange(temp_userid);
                    }
                }
                if (receiver.ToRoleNames?.Any() ?? false)//从roleName获取userid
                {
                    var roleList = await roleClient.GetRolesAsync(token);
                    foreach (var roleName in receiver.ToRoleNames)
                    {
                        long roleId = roleList.Find(c => c.Key == roleName).Value;
                        if (roleId != 0)
                        {
                            List<string>? temp_userid = await memoryCache.GetOrCreateAsync($"dingding - {roleId}", async (e) =>
                            {
                                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
                                return await roleClient.GetUserIdbyRoleIdAsync(token, roleId);
                            });
                            //List<string>? temp_userid = await roleClient.GetUserIdbyRoleIdAsync(token, roleId);
                            if (temp_userid?.Any() ?? false) users.AddRange(temp_userid);
                        }
                    }
                }
                if (receiver.ToUserMobiles?.Any() ?? false)//从手机号获取userid
                {
                    foreach (string mobile in receiver.ToUserMobiles)
                    {
                        string? temp_userid = await memoryCache.GetOrCreateAsync($"dingding - {mobile}", async (e) =>
                        {
                            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
                            return await userClient.GetUseridbyMobileAsync(token, mobile);
                        });
                        //string? temp_userid = await userClient.GetUseridbyMobileAsync(token, mobile);
                        if (!string.IsNullOrEmpty(temp_userid)) users.Add(temp_userid);
                    }
                }
                var result = "";
                if (users.Any())
                    result = string.Join(",", users);
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 确认消息接收方(部门)
        /// </summary>
        private async Task<string> DingDepartmentFilter(string token, UnparsedReceiver receiver)
        {
            try
            {
                var departments = new List<long>();
                if (receiver.ToDepartmentIds?.Any() ?? false)
                {
                    departments.AddRange(receiver.ToDepartmentIds);
                }

                if (receiver.ToDepartmentNames?.Any() ?? false)
                {
                    foreach (var departmentName in receiver.ToDepartmentNames)
                    {
                        long temp_departmentid = await memoryCache.GetOrCreateAsync($"dingding - {departmentName}", async (e) =>
                        {
                            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
                            return await departmentClient.GetDepartmentIdAysnc(token, departmentName);
                        });
                        //long temp_departmentid = await departmentClient.GetDepartmentIdAysnc(token, departmentName);
                        if (temp_departmentid != 0) departments.Add(temp_departmentid);
                    }
                }

                var result = "";
                if (departments.Any())
                    result = string.Join(",", departments);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}