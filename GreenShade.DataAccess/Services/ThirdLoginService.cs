using GreenShade.Blog.Domain.Models;
using GreenShade.Blog.Domain.OAuth;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
namespace GreenShade.Blog.DataAccess.Services
{
    public class ThirdLoginService
    {
        protected HttpClient Backchannel => new HttpClient();
        public IConfiguration Configuration { get; }
        public ThirdLoginService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<string> QQLogin(string code)
        {
            var qqSettings = new QQLoginSetting();
            //绑定jwtSeetings
            Configuration.Bind("qqlogin", qqSettings);
            QQAuthenticationOptions qQAuthentication = new QQAuthenticationOptions();
            qQAuthentication.ClientId = qqSettings.client_id;
            qQAuthentication.ClientSecret = qqSettings.client_secret;
            var tokens = await ExchangeCodeAsync(qQAuthentication,code);
            return "";
        }

        protected virtual async Task<OAuthTokenResponse> ExchangeCodeAsync(QQAuthenticationOptions Options,string code)
        {
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", Options.ClientId },
                { "redirect_uri", "https://www.xworldstudio.cn/signin-qq"},
                { "client_secret", Options.ClientSecret },
                { "code", "C237FB159DA50FE4500EB9FE6E16B1DF" },
                { "grant_type", "authorization_code" },
            };
          
            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = requestContent;
            var response = await Backchannel.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                var payload = Newtonsoft.Json.Linq.JObject.Parse(await response.Content.ReadAsStringAsync());
                return OAuthTokenResponse.Success(payload);
            }
            else
            {
                return OAuthTokenResponse.Failed(new Exception());
            }
        }
    }
}
