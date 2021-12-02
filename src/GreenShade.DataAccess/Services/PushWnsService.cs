using GreenShade.Blog.DataAccess.Data;
using GreenShade.Blog.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
namespace GreenShade.Blog.DataAccess.Services
{
    // Authorization
    [DataContract]
    public class OAuthToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
    }

    public class PushWnsService
    {
        private readonly HttpClient _httpClient;
        public PushWnsService(HttpClient httpClient,
            BlogSysContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }
        private readonly BlogSysContext _context;

        public async Task<bool> InsertWnsPushUrl(WnsPushUrl wnsPushUrl)
        {
            bool ret = false;
            try
            {
                wnsPushUrl.CreateDate = DateTime.Now;
                wnsPushUrl.UpdateDate = DateTime.Now;
                _context.WnsUrls.Add(wnsPushUrl);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch (Exception ex)
            {

            }
            return ret;
        }
        public async Task<bool> UpdateWnsPushUrl(WnsPushUrl wnsPushUrl)
        {
            bool ret = false;
            try
            {
                var wns=_context.WnsUrls.Where(w =>w.AppName==wnsPushUrl.AppName && w.DevFamily == wnsPushUrl.DevFamily).FirstOrDefault();
                if (wns != null)
                {
                    wns.UpdateDate = DateTime.Now;
                    wns.PushUrl = wnsPushUrl.PushUrl;
                    _context.WnsUrls.Update(wns);
                    await _context.SaveChangesAsync();
                    ret = true;
                }               
            }
            catch (Exception ex)
            {

            }
            return ret;
        }
        // Post to WNS
        public async Task<string> PostToWnsAsync(string secret, string sid, string uri, string xml, string notificationType, string contentType)
        {
            try
            {
                // You should cache this access token.
                var accessToken = await GetAccessTokenAsync(secret, sid);


                _httpClient.DefaultRequestHeaders.Add("X-WNS-Type", notificationType);
                _httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", accessToken.AccessToken));
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                //_httpClient.DefaultRequestHeaders.Add("Content-Type", contentType);
                byte[] contentInBytes = Encoding.UTF8.GetBytes(xml);
                //StringContent stringContent = new StringContent(xml,Encoding.UTF8);
                ByteArrayContent byteArray = new ByteArrayContent(contentInBytes);
                var res = await _httpClient.PostAsync(uri, byteArray);
                var rescontent = res.Content.ReadAsStringAsync();
                return res.StatusCode.ToString();
                //byte[] contentInBytes = Encoding.UTF8.GetBytes(xml);

                //HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
                //request.Method = "POST";
                //request.Headers.Add("X-WNS-Type", notificationType);
                //request.ContentType = contentType;
                //request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken.AccessToken));

                //using (Stream requestStream = request.GetRequestStream())
                //    requestStream.Write(contentInBytes, 0, contentInBytes.Length);

                //using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                //return webResponse.StatusCode.ToString();
            }

            catch (WebException webException)
            {
                HttpStatusCode status = ((HttpWebResponse)webException.Response).StatusCode;

                if (status == HttpStatusCode.Unauthorized)
                {
                    // The access token you presented has expired. Get a new one and then try sending
                    // your notification again.

                    // Because your cached access token expires after 24 hours, you can expect to get 
                    // this response from WNS at least once a day.

                    await GetAccessTokenAsync(secret, sid);

                    // We recommend that you implement a maximum retry policy.
                    string x = await PostToWnsAsync(uri, xml, secret, sid, notificationType, contentType);
                    return x;
                }
                else if (status == HttpStatusCode.Gone || status == HttpStatusCode.NotFound)
                {
                    // The channel URI is no longer valid.

                    // Remove this channel from your database to prevent further attempts
                    // to send notifications to it.

                    // The next time that this user launches your app, request a new WNS channel.
                    // Your app should detect that its channel has changed, which should trigger
                    // the app to send the new channel URI to your app server.

                    return "";
                }
                else if (status == HttpStatusCode.NotAcceptable)
                {
                    // This channel is being throttled by WNS.

                    // Implement a retry strategy that exponentially reduces the amount of
                    // notifications being sent in order to prevent being throttled again.

                    // Also, consider the scenarios that are causing your notifications to be throttled. 
                    // You will provide a richer user experience by limiting the notifications you send 
                    // to those that add true value.

                    return "";
                }
                else
                {
                    // WNS responded with a less common error. Log this error to assist in debugging.

                    // You can see a full list of WNS response codes here:
                    // http://msdn.microsoft.com/en-us/library/windows/apps/hh868245.aspx#wnsresponsecodes

                    string[] debugOutput = {
                                       status.ToString(),
                                       webException.Response.Headers["X-WNS-Debug-Trace"],
                                       webException.Response.Headers["X-WNS-Error-Description"],
                                       webException.Response.Headers["X-WNS-Msg-ID"],
                                       webException.Response.Headers["X-WNS-Status"]
                                   };
                    return string.Join(" | ", debugOutput);
                }
            }

            catch (Exception ex)
            {
                return "EXCEPTION: " + ex.Message;
            }
        }

        // Authorization
        [DataContract]
        public class OAuthToken
        {
            [DataMember(Name = "access_token")]
            public string AccessToken { get; set; }
            [DataMember(Name = "token_type")]
            public string TokenType { get; set; }
        }

        private OAuthToken GetOAuthTokenFromJson(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                var ser = new DataContractJsonSerializer(typeof(OAuthToken));
                var oAuthToken = (OAuthToken)ser.ReadObject(ms);
                return oAuthToken;
            }
        }

        protected async Task<OAuthToken> GetAccessTokenAsync(string secret, string sid)
        {
            IDictionary<string, string> keyValues = new Dictionary<string, string>()
            {
                {"grant_type","client_credentials" },
                {"client_id",sid },
                {"client_secret",secret },
                {"scope","notify.windows.com" }
            };
            var postContent = new FormUrlEncodedContent(keyValues);
            string response;

            var responseMessage = _httpClient.PostAsync("https://login.live.com/accesstoken.srf", postContent);
            response = await responseMessage.Result.Content.ReadAsStringAsync();
            return GetOAuthTokenFromJson(response);
        }

    }
}
