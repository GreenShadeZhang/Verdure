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
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace GreenShade.Blog.Api.Services
{
    public class ThirdLoginService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly QQLoginSetting _qqSettings;
        private readonly HttpClient _http;
        private ILogger<ThirdLoginService> _logger;
        public IConfiguration Configuration { get; }
        public ThirdLoginService(IConfiguration configuration, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
             IOptions<QQLoginSetting> qqSettingsOptions,
             HttpClient http,
             ILogger<ThirdLoginService> logger)
        {
            Configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _qqSettings = qqSettingsOptions.Value;
            _http = http;
            _logger = logger;
        }
        public async Task<ApplicationUser> QQLogin(string code)
        {
            _logger.LogInformation("welcome");
            var accessToken = await ExchangeCodeAsync(code);
            return await GetUserInfoAsync(accessToken);
        }

        private async Task<ApplicationUser> GetUserInfoAsync(OAuthTokenResponse accessToken)
        {
            ApplicationUser applicationUser = null;
            string openid = await GetUserIdentifierAsync(accessToken);
            _logger.LogInformation(openid);
            if (!string.IsNullOrWhiteSpace(openid))
            {
                applicationUser = await _userManager.FindByLoginAsync("QQ", openid);
                if (applicationUser != null)
                {
                    return applicationUser;
                    //var res=  await _signInManager.ExternalLoginSignInAsync("QQ", openid, true);
                    // return res.Succeeded ? applicationUser : null;                   
                }
            }
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "oauth_consumer_key", _qqSettings.client_id},
                { "access_token", accessToken.AccessToken},
                { "openid", openid }
            };
            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
            var response = await _http.PostAsync(QQAuthenticationDefaults.UserInformationEndpoint, requestContent);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRequestException("An error occurred while retrieving the user identifier.");
            }
            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            int status = payload.RootElement.GetProperty("ret").GetInt32();
            if (status != 0)
            {
                return new ApplicationUser();
                //throw new HttpRequestException("An error occurred while retrieving user information.");
            }
            QQUserInfo ret = Newtonsoft.Json.JsonConvert.DeserializeObject<QQUserInfo>(await response.Content.ReadAsStringAsync());
            applicationUser = new ApplicationUser()
            {
                NickName = ret.nickname,
                UserName = GetRandomString(9),
                Province = ret.province,
                City = ret.city,
                Gender = ret.gender,
                GenderType = ret.gender_type,
                Avatar = ret.figureurl_qq,
                Year = ret.year

            };
            var res = await _userManager.CreateAsync(applicationUser);
            if (res.Succeeded && !string.IsNullOrWhiteSpace(openid))
            {
                UserLoginInfo userLogin = new UserLoginInfo("QQ", openid, "QQ");
                await _userManager.AddLoginAsync(applicationUser, userLogin);
            }
            return applicationUser;
        }


        public static string GetRandomString(int length)
        {
            string randStr = "";
            Random rd = new Random();
            byte[] str = new byte[length];
            int i;
            for (i = 0; i < length - 1; i++)
            {
                int a = 0;
                while (!((a >= 48 && a <= 57) || (a >= 97 && a <= 122)))
                {
                    a = rd.Next(48, 122);
                }
                str[i] = (byte)a;
            }
            string username = new string(UnicodeEncoding.ASCII.GetChars(str));
            Random r = new Random(unchecked((int)DateTime.Now.Ticks));
            string s1 = ((char)r.Next(97, 122)).ToString();
            username = username.Replace("\0", "");
            randStr = s1 + username;
            return randStr;
        }
        private async Task<string> GetUserIdentifierAsync(OAuthTokenResponse tokens)
        {
            string address = string.Format(QQAuthenticationDefaults.UserIdentificationEndpoint + "?access_token={0}", tokens.AccessToken);
            var response = await _http.GetAsync(address);
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

        protected virtual async Task<OAuthTokenResponse> ExchangeCodeAsync(string code)
        {
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", _qqSettings.client_id},
                { "redirect_uri", _qqSettings.redirect_uri},
                { "client_secret", _qqSettings.client_secret },
                { "code", code },
                { "grant_type", "authorization_code" },
            };
            _logger.LogInformation("client_id"+_qqSettings.client_id);
            _logger.LogInformation("redirect_uri" + _qqSettings.redirect_uri);
            _logger.LogInformation("secret" + _qqSettings.client_secret);
            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
            var response = await _http.PostAsync(QQAuthenticationDefaults.TokenEndpoint, requestContent);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = QueryHelpers.ParseQuery(await response.Content.ReadAsStringAsync());
                _logger.LogInformation("res_ex", content);
                var payload = await CopyPayloadAsync(content);
                return OAuthTokenResponse.Success(payload);
            }
            else
            {
                return OAuthTokenResponse.Failed(new System.Exception());
            }
        }
        /// <summary>
        /// 工具类 将键值对转换成json 针对获取的access_token
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
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
