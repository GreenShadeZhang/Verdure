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

namespace GreenShade.Blog.Api.Services
{
    public class ThirdLoginService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public IConfiguration Configuration { get; }
        QQLoginSetting qqSettings = new QQLoginSetting();
        public ThirdLoginService(IConfiguration configuration,SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            Configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            //绑定jwtSeetings
            Configuration.Bind("qqlogin", qqSettings);
        }
        public async Task<ApplicationUser> QQLogin(string code)
        {
          var accessToken= await ExchangeCodeAsync(code);
          var info=  await GetUserInfoAsync(accessToken);  
           return info;
        }

        private async Task<ApplicationUser> GetUserInfoAsync(OAuthTokenResponse accessToken)
        {
            ApplicationUser applicationUser = null;
            string openid = await GetUserIdentifierAsync(accessToken);
            if (!string.IsNullOrWhiteSpace(openid))
            {
                applicationUser = await _userManager.FindByLoginAsync("QQ", openid);
                if (applicationUser != null)
                {
                    return applicationUser;
                }
            }            
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "oauth_consumer_key", qqSettings.client_id},
                { "access_token", accessToken.AccessToken},
                { "openid", openid }            
            };
            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
            using (var http = new HttpClient())
            {
                var response = await http.PostAsync(QQAuthenticationDefaults.UserInformationEndpoint, requestContent);
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
                { NickName = ret.nickname, 
                    UserName = GetRandomString(9), 
                    Province=ret.province,
                     City=ret.city,
                      Gender=ret.gender,
                      GenderType=ret.gender_type,
                       Avatar=ret.figureurl_qq,
                        Year=ret.year
                    
                };
                var res= await  _userManager.CreateAsync(applicationUser);
                if (res.Succeeded &&!string.IsNullOrWhiteSpace(openid))
                {
                    UserLoginInfo userLogin = new UserLoginInfo("QQ",openid,"QQ");
                   await _userManager.AddLoginAsync(applicationUser, userLogin);                   
                }
                return applicationUser;
            }
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
