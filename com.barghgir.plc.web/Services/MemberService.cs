using com.barghgir.plc.data.Models;
using com.barghgir.plc.infra.common.Encryption;
using com.barghgir.plc.web.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.Services
{
    public class MemberService : BaseService
    {
        private readonly ILogger<MemberService> logger;
        private readonly HttpClient httpClient;

        public MemberService(
            IConfigurationService configurationService,
            ILogger<MemberService> logger) : base(configurationService)
        {
            this.logger = logger;
            this.httpClient = HttpHelper.GetHttpClient();
        }

        //public static async Task<string> BaseAddress() =>
        //    (await ConfigurationService.GetEnvironment())?.Options?.BaseServiceEndpoint;

        public string lastUrl { get; set; }

        public async Task<string> SignIn(string username, string password)
        {
            string token = null; //adam%40adamcox.net
            try
            {
                // TODO: make a helper or service to get keyvault secrets
                string key = AppConfig.Security.AesEncryptionKey; // "Bj/ocRz0FtJR0n7LjArcMabp9bi1qGXqQuaqXIJePmw="; // todo: get from config via api call (figure out state or store in local secure storage)
                string vector = AppConfig.Security.AesEncryptionIVector; // "XtQAltYf3Rj55G1iOiXgWw==";  // todo: get from config via api call
                string protectedPassword = DataProtectionHelper.EncryptDataWithAes(password.Trim(), ref key, ref vector); // RJG3LiZaUY9KuAXOqTli6KMymD2tklDjkZTZb3f3k4E=
           
                var encodedUsername = WebUtility.UrlEncode(username);
                var encodedProtectedPassword = WebUtility.UrlEncode(protectedPassword);
                var url = $"{BaseAddress}/community/auth/signin?email={encodedUsername}&password={encodedProtectedPassword}&isPasswordClear=false";
                lastUrl = url;
                string json = JsonSerializer.Serialize(new {
                    username,
                    password = protectedPassword
                });
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, null);

                if (!response.IsSuccessStatusCode)
                {
                    // TODO: figure out how to send notifications to present user with feedback like "successful" or "error"
                    logger.LogError("{method} failed: {message}", nameof(SignIn), response.Content);
                    return null;
                }
                token = await response.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {
                logger.LogError("{method} raised exception: {exception}", nameof(SignIn), ex);
            }
            return token;
        }

        public async Task<List<Member>> GetMemberListAsync()
        {
            var url = $"{BaseAddress}/community/admin/member/list"; // "https://10.0.2.2:5001/course/list";
            Console.WriteLine($"GET {url}");
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"{nameof(GetMemberListAsync)} ERROR: {response.ReasonPhrase}");
            var result = await response.Content.ReadFromJsonAsync<List<Member>>();
            return result;
        }
    }
}
