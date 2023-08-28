
namespace CloudPlatformMessageProvider
{
    public class DingTalkPlatformOptions
    {
        public DingTalkPlatformOptions() { }
        public DingTalkPlatformOptions(string dingWorkNoticeUrl, string dingGetAccessTokenUrl, string dingGetSubDepartmentsUrl, string dinGetUserIdbyDepartmentsUrl, string dinGetRolesUrl, string dingGetUserIdbyRoleIdUrl, string dingGetUserIdbyMobilesUrl)
        {
            DingWorkNoticeUrl = dingWorkNoticeUrl;
            DingGetAccessTokenUrl = dingGetAccessTokenUrl;
            DingGetSubDepartmentsUrl = dingGetSubDepartmentsUrl;
            DinGetUserIdbyDepartmentsUrl = dinGetUserIdbyDepartmentsUrl;
            DinGetRolesUrl = dinGetRolesUrl;
            DingGetUserIdbyRoleIdUrl = dingGetUserIdbyRoleIdUrl;
            DingGetUserIdbyMobilesUrl = dingGetUserIdbyMobilesUrl;
        }

        public string DingWorkNoticeUrl { get; init; } = "https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token=";
        public string DingGetAccessTokenUrl { get; init; } = "https://api.dingtalk.com/v1.0/oauth2/accessToken";
        public string DingGetSubDepartmentsUrl { get; init; } = "https://oapi.dingtalk.com/topapi/v2/department/listsub?access_token=";
        public string DinGetUserIdbyDepartmentsUrl { get; init; } = "https://oapi.dingtalk.com/topapi/user/listid?access_token=";
        public string DinGetRolesUrl { get; init; } = "https://oapi.dingtalk.com/topapi/role/list?access_token=";
        public string DingGetUserIdbyRoleIdUrl { get; init; } = "https://oapi.dingtalk.com/topapi/role/simplelist?access_token=";
        public string DingGetUserIdbyMobilesUrl { get; init; } = "https://oapi.dingtalk.com/topapi/v2/user/getbymobile?access_token=";
    }
}