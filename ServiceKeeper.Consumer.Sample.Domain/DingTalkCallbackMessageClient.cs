using CloudPlatformMessageProvider.CloudPlatform;
using ServiceKeeper.Consumer.Sample.Domain;
using ServiceKeeper.Consumer.Sample.Domain.Entity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceKeeper.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace ServiceKeeper.Consumer.Sample.Domain
{
    //public class DingTalkCallbackMessageClient : ICallbackClient
    //{
    //    private readonly DingCallbackOptions options;
    //    private readonly IDistrubutedCatchExtensions distrubutedCatch;
    //    private readonly DingMessageClient messageClient;
    //    private readonly DingAccessTokenClient accessTokenClient;
    //    private readonly DingUserClient userClient;
    //    public DingTalkCallbackMessageClient(DingCallbackOptions options, IDistrubutedCatchExtensions distrubutedCatch, DingMessageClient messageClient, DingAccessTokenClient accessTokenClient, DingUserClient userClient)
    //    {
    //        this.options = options;
    //        this.distrubutedCatch = distrubutedCatch;
    //        this.messageClient = messageClient;
    //        this.accessTokenClient = accessTokenClient;
    //        this.userClient = userClient;
    //    }

    //    public async Task<bool> InfoCallback(TaskDetail taskDetail)
    //    {
    //        try
    //        {
    //            string token = await GetToken();
    //            string usersid = await GetReveiver(taskDetail, token);
    //            string json = JsonConvert.SerializeObject(new
    //            {
    //                msg = new
    //                {
    //                    action_card = new
    //                    {
    //                        single_url = options.ConfigurationUrl,
    //                        single_title = options.CallbackTilte,
    //                        markdown = $"### 任务: {taskDetail.TaskName} 回调消息通知:\n\n#### {taskDetail.CallbackMessage}",
    //                        title = $"{taskDetail.TaskName}消息发送失败"
    //                    },
    //                    msgtype = "action_card"
    //                },
    //                to_all_user = "false",
    //                agent_id = options.Agentid,
    //                userid_list = usersid
    //            });
    //            return await messageClient.SendMessageNoticeAsync(token, json);
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.GetInstance().Log_error($"回调发送失败,任务名:{taskDetail.TaskName} 错误信息:{ex.Message}");
    //            return false;
    //        }
    //    }

    //    public async Task<bool> ErrorCallback(TaskDetail taskDetail)
    //    {
    //        try
    //        {
    //            string token = await GetToken();
    //            string usersid = await GetReveiver(taskDetail, token);
    //            string json = JsonConvert.SerializeObject(new
    //            {
    //                msg = new
    //                {
    //                    action_card = new
    //                    {
    //                        single_url = options.ConfigurationUrl,
    //                        single_title = options.CallbackTilte,
    //                        markdown = $"### {taskDetail.TaskName} 消息发送失败\n\n#### 错误信息: \n\n##### {taskDetail.CallbackMessage}",
    //                        title = $"{taskDetail.TaskName}消息发送失败"
    //                    },
    //                    msgtype = "action_card"
    //                },
    //                to_all_user = "false",
    //                agent_id = options.Agentid,
    //                userid_list = usersid
    //            });
    //            return await messageClient.SendMessageNoticeAsync(token, json);
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.GetInstance().Log_error($"回调发送失败,任务名:{taskDetail.TaskName} 错误信息:{ex.Message}");
    //            return false;
    //        }
    //    }


    //    private async Task<string> GetToken()
    //    {
    //        string? token = await distrubutedCatch.GetOrCreateAsync($"dingding - {options.Corpid} - {options.Corpsecret}", async (e) =>
    //        {
    //            return await accessTokenClient.GetAccessTokenAsync(options.Corpid, options.Corpsecret);
    //        }, 3600);
    //        return token!;
    //    }

    //    private async Task<string> GetReveiver(TaskDetail taskDetail, string token)
    //    {

    //        List<string> usersid = new();
    //        foreach (string mobile in taskDetail.TaskCallbackReceivers)
    //        {
    //            string? temp_userid = await distrubutedCatch.GetOrCreateAsync($"dingding - {mobile}", async (e) =>
    //            {
    //                return await userClient.GetUseridbyMobileAsync(token, mobile);
    //            }, 3600);
    //            if (!string.IsNullOrEmpty(temp_userid)) usersid.Add(temp_userid);
    //        }

    //        if (!usersid.Any()) throw new Exception($"找不到回调目标手机号对应的 UserId");

    //        string result = string.Join(",", usersid);
    //        return result;
    //    }
    //}

}
