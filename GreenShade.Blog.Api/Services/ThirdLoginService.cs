using GreenShade.Blog.Domain.Models;
using GreenShade.Blog.Domain.OAuth;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Buffers;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using System;

namespace GreenShade.Blog.Api.Services
{
    public class ThirdLoginService
    {
        protected HttpClient Backchannel => new HttpClient();
        public IConfiguration Configuration { get; }
        QQLoginSetting qqSettings = new QQLoginSetting();
        public ThirdLoginService(IConfiguration configuration)
        {
            Configuration = configuration;

            //绑定jwtSeetings
            Configuration.Bind("qqlogin", qqSettings);
        }
        public async Task<QQUserInfo> QQLogin(string code)
        {
          var accessToken= await ExchangeCodeAsync(code);
          var info=  await GetUserInfoAsync(accessToken);  
           return info;
        }

        private async Task<QQUserInfo> GetUserInfoAsync(OAuthTokenResponse accessToken)
        {
            QQUserInfo ret = null;
            string openid = await GetUserIdentifierAsync(accessToken);
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "oauth_consumer_key", qqSettings.client_id},
                { "access_token", accessToken.AccessToken},
                { "openid", openid }            
            };
            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await http.PostAsync(QQAuthenticationDefaults.UserInformationEndpoint, requestContent);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new HttpRequestException("An error occurred while retrieving the user identifier.");
                }
                using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

                int status = payload.RootElement.GetProperty("ret").GetInt32();
                if (status != 0)
                {
                    return ret;
                    //throw new HttpRequestException("An error occurred while retrieving user information.");
                }
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<QQUserInfo>(await response.Content.ReadAsStringAsync());
                return ret;
            }
        }

        private async Task<string> GetUserIdentifierAsync(OAuthTokenResponse tokens)
        {
            string address =string.Format(QQAuthenticationDefaults.UserIdentificationEndpoint+ "?access_token={0}", tokens.AccessToken);
            using (var http = new HttpClient())
            {           
                var response = await http.GetAsync(address);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new HttpRequestException("An error occurred while retrieving the user identifier.");
                }             
                string body = await response.Content.ReadAsStringAsync();
                int index = body.IndexOf("{");
                if (index > 0)
                {
                    body = body.Substring(index, body.LastIndexOf("}") - index + 1);
                }
                using var payload = JsonDocument.Parse(body);
                return payload.RootElement.GetString("openid");
            }
            
        }

        protected virtual async Task<OAuthTokenResponse> ExchangeCodeAsync(string code)
        {   
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", qqSettings.client_id},
                { "redirect_uri", qqSettings.redirect_uri},
                { "client_secret", qqSettings.client_secret },
                { "code", code },
                { "grant_type", "authorization_code" },
            };
            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await http.PostAsync(QQAuthenticationDefaults.TokenEndpoint, requestContent);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = QueryHelpers.ParseQuery(await response.Content.ReadAsStringAsync());
                    var payload = await CopyPayloadAsync(content);
                    return OAuthTokenResponse.Success(payload);
                }
                else
                {
                    return OAuthTokenResponse.Failed(new System.Exception());
                }
            }
        }
        private async Task<JsonDocument> CopyPayloadAsync(Dictionary<string, StringValues> content)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();

            await using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                writer.WriteStartObject();

                foreach (var item in content)
                {
                    writer.WriteString(item.Key, item.Value);
                }

                writer.WriteEndObject();
                await writer.FlushAsync();
            }

            return JsonDocument.Parse(bufferWriter.WrittenMemory);
        }
    }
}
