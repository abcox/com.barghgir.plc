using com.barghgir.plc.data.Models;
using com.barghgir.plc.web.Helpers;
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
    public class MemberService
    {
        public MemberService()
        {
        }

        public static async Task<string> BaseAddress() =>
            (await ConfigurationService.GetEnvironment())?.Options?.BaseServiceEndpoint;

        public async Task<string> SignIn(string username, string protectedPassword)
        {
            string token; //adam%40adamcox.net
            var encodedUsername = WebUtility.UrlEncode(username);
            var encodedProtectedPassword = WebUtility.UrlEncode(protectedPassword);
            var url = $"{await BaseAddress()}/community/auth/signin?email={encodedUsername}&password={encodedProtectedPassword}&isPasswordClear=false";
            string json = JsonSerializer.Serialize(new {
                username,
                password = protectedPassword
            });
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await HttpHelper.GetHttpClient().PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                // TODO: figure out how to send notifications to present user with feedback like "successful" or "error"
                return null;
            }
            token = await response.Content.ReadAsStringAsync();
            return token;
        }
    }
}
