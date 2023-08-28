using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace CloudPlatformMessageProvider.CloudPlatform
{
    public class DingTalkBaseClient : IDisposable
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly HttpClient httpClient;


        public DingTalkBaseClient(IHttpClientFactory httpClientFactory)
        {
            clientFactory = httpClientFactory;
            httpClient = clientFactory.CreateClient();
        }

        public void Dispose()
        {
            httpClient.Dispose();
            GC.SuppressFinalize(this);
        }

        protected async Task<HttpResponseMessage> ExecuteAsync(string url, string json)
        {
            HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            return response;
        }

        protected async Task<HttpResponseMessage> ExecuteAsync(string url)
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            return response;
        }
    }
    /// <summary>
    /// 钉钉Token
    /// </summary>
    public class DingAccessTokenClient : DingTalkBaseClient
    {
        public DingAccessTokenClient(IHttpClientFactory httpClientFactory, DingTalkPlatformOptions dingTalkOptions) : base(httpClientFactory) { options = dingTalkOptions; }
        private readonly DingTalkPlatformOptions options;

        public async Task<string> GetAccessTokenAsync(string corpid, string corpsecret)
        {
            string url = options.DingGetAccessTokenUrl;
            string json = JsonSerializer.Serialize(new
            {
                appKey = corpid,
                appSecret = corpsecret,
            });

            using HttpResponseMessage response = await ExecuteAsync(url, json);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<dynamic?>(responseContent)?["accessToken"].ToString() ?? throw new Exception($"使用corpid为{corpid}的GetAccessToken失败");
            }
            else
            {
                throw new Exception($"使用corpid为{corpid}的GetAccessToken失败");
            }

        }
    }

    /// <summary>
    /// 钉钉用户类
    /// </summary>
    public class DingUserClient : DingTalkBaseClient
    {
        public DingUserClient(IHttpClientFactory httpClientFactory, DingTalkPlatformOptions dingTalkOptions) : base(httpClientFactory) { options = dingTalkOptions; }
        private readonly DingTalkPlatformOptions options;
        /// <summary>
        /// 通过手机号或取用户id
        /// </summary>
        public async Task<string> GetUseridbyMobileAsync(string token, string mobile)
        {
            string result = "";
            string url = options.DingGetUserIdbyMobilesUrl + token;

            var json = JsonSerializer.Serialize(new { mobile });
            using HttpResponseMessage response = await ExecuteAsync(url, json);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic? dyn = JsonSerializer.Deserialize<dynamic>(responseContent);
                if ((int)dyn?["errcode"] == 0)
                {
                    result = dyn["result"]["userid"].ToString();
                }
            }
            return result;

        }
        /// <summary>
        /// 通过手机号或取用户id
        /// </summary>
        public async Task<List<string>> GetUseridbyMobilesAsync(string token, List<string> mobiles)
        {
            List<string> result = new();
            string url = options.DingGetUserIdbyMobilesUrl + token;

            foreach (string mobile in mobiles)
            {
                var json = JsonSerializer.Serialize(new { mobile });
                using HttpResponseMessage response = await ExecuteAsync(url, json);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic? dyn = JsonSerializer.Deserialize<dynamic>(responseContent);
                    if ((int)dyn?["errcode"] == 0)
                    {
                        result.Add(dyn["result"]["userid"].ToString());
                    }
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 钉钉部门类
    /// </summary>
    public class DingDepartmentClient : DingTalkBaseClient
    {
        public DingDepartmentClient(IHttpClientFactory httpClientFactory, DingTalkPlatformOptions dingTalkOptions) : base(httpClientFactory) { options = dingTalkOptions; }
        private readonly DingTalkPlatformOptions options;
        /// <summary>
        /// 获取下属一级部门列表
        /// </summary>
        /// <param name="dept_id">部门id,root为1</param>
        public async Task<List<KeyValuePair<string, int>>> GetSubDepartmentsAsync(string token, long dept_id = 1)
        {
            var url = $"{options.DingGetSubDepartmentsUrl}{token}";
            var json = JsonSerializer.Serialize(new
            {
                language = "zh_CN",
                dept_id
            });
            using var response = await ExecuteAsync(url, json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"返回错误码：{response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic? res = JsonSerializer.Deserialize<dynamic>(responseContent);

            if ((int)res?["errcode"] != 0)
            {
                throw new Exception(res?["errmsg"].ToString());
            }

            var subDepartments = new List<KeyValuePair<string, int>>();
            foreach (var item in res["result"])
            {
                subDepartments.Add(new KeyValuePair<string, int>((string)item.name, (int)item.dept_id));
            }

            return subDepartments;

        }

        /// <summary>
        /// 获取下属所有部门列表
        /// </summary>
        /// <param name="dept_id">部门id,root为1</param>
        public async Task<Dictionary<string, int>> GetAllDepartmentIdsAsync(string token, long dept_id)
        {

            var departmentIds = new Dictionary<string, int>();
            var subDepartments = await GetSubDepartmentsAsync(token, dept_id);
            foreach (var subDepartment in subDepartments)
            {
                departmentIds[subDepartment.Key] = subDepartment.Value;
                var subDepartmentIds = await GetAllDepartmentIdsAsync(token, subDepartment.Value);
                foreach (var subDepartmentId in subDepartmentIds)
                {
                    departmentIds[subDepartmentId.Key] = subDepartmentId.Value;
                }
            }
            return departmentIds;
        }

        /// <summary>
        /// 使用递归，从传入的部门id开始递归查找下属部门，找到所需的部门后返回部门的ID，如果未找到该部门，则返回0
        /// </summary>
        /// <param name="dept_id">部门id,root为1</param>
        /// <param name="dept_name">需查找的部门名称</param>
        /// <returns>部门id</returns>
        public async Task<long> GetDepartmentIdAysnc(string token, string dept_name, long dept_id = 1)
        {
            var departmentIds = await GetAllDepartmentIdsAsync(token, dept_id);
            if (departmentIds.TryGetValue(dept_name, out int id))
            {
                return id;
            }
            return 0;

        }
    }

    /// <summary>
    /// 钉钉角色类
    /// </summary>
    public class DingRoleClient : DingTalkBaseClient
    {
        public DingRoleClient(IHttpClientFactory httpClientFactory, DingTalkPlatformOptions dingTalkOptions) : base(httpClientFactory) { options = dingTalkOptions; }
        private readonly DingTalkPlatformOptions options;
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="is_reload">是否清空缓存重新加载</param>
        public async Task<List<KeyValuePair<string, long>>> GetRolesAsync(string token)
        {

            var url = $"{options.DinGetRolesUrl}{token}";
            using HttpResponseMessage response = await ExecuteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic? allRoleGroup = JsonSerializer.Deserialize<dynamic>(responseContent);
                if ((int)allRoleGroup?["errcode"] == 0)
                {
                    var result = new List<KeyValuePair<string, long>>();
                    foreach (var roleGroup in allRoleGroup["result"]["list"])
                    {
                        foreach (var roles in roleGroup["roles"])
                        {
                            //key重复
                            result.Add(new KeyValuePair<string, long>((string)roles["name"], Convert.ToInt64(roles["id"])));
                        }
                    }
                    return result;
                }
                else
                {
                    throw new Exception(allRoleGroup?["errmsg"].ToString() ?? "返回errmsg为空");
                }
            }
            else
            {
                throw new Exception($"返回错误码：{response.StatusCode}");
            }

        }

        /// <summary>
        /// 获取指定角色的员工列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="roleid">角色id</param>
        /// <returns></returns>
        public async Task<List<string>> GetUserIdbyRoleIdAsync(string token, long roleid)
        {

            var url = $"{options.DingGetUserIdbyRoleIdUrl}{token}";

            var json = JsonSerializer.Serialize(new { role_id = roleid });
            using var response = await ExecuteAsync(url, json);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic? roleResult = JsonSerializer.Deserialize<dynamic>(responseContent);
                if ((int)roleResult?["errcode"] == 0)
                {
                    var result = new List<string>();
                    foreach (var item in roleResult["result"]["list"])
                    {
                        result.Add(item["userid"].ToString());
                    }
                    return result;
                }
                else
                {
                    throw new Exception(roleResult?["errmsg"].ToString() ?? "返回errmsg为空");
                }
            }
            else
            {
                throw new Exception($"返回错误码：{response.StatusCode}");
            }

        }
    }

    public class DingMessageClient : DingTalkBaseClient
    {
        public DingMessageClient(IHttpClientFactory httpClientFactory, DingTalkPlatformOptions dingTalkOptions) : base(httpClientFactory) { options = dingTalkOptions; }
        private readonly DingTalkPlatformOptions options;
        public async Task<bool> SendMessageRobotAsync(string url, string json)
        {
            using HttpResponseMessage response = await ExecuteAsync(url, json);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<dynamic>(responseContent);
                if ((int)result?["errcode"] == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> SendMessageNoticeAsync(string token, string json)
        {
            string url = $"{options.DingWorkNoticeUrl}{token}";
            using HttpResponseMessage response = await ExecuteAsync(url, json);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<dynamic>(responseContent);
                if ((int)result?["errcode"] == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}