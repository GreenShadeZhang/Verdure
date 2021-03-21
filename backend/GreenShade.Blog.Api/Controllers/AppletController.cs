using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GreenShade.Blog.Api.Common;
using GreenShade.Blog.Api.Filters;
using GreenShade.Blog.DataAccess.Data;
using GreenShade.Blog.DataAccess.Services;
using GreenShade.Blog.Domain;
using GreenShade.Blog.Domain.Dto;
using GreenShade.Blog.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GreenShade.Blog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppletController : ControllerBase
    {
        public AppletController(IConfiguration configuration,
            PushWnsService pushWnsService,
            WallpaperService wallpaperService,
            BlogSysContext context,
             ILogger<AppletController> logger,
             IOptions<Dictionary<string, WnsSetting>> wnsSettingsOptions)
        {
            PushWnsService = pushWnsService;
            Configuration = configuration;
            WallpaperService = wallpaperService;
            _wnsSetting = wnsSettingsOptions.Value;
            _context = context;
            _logger = logger;
        }
        private ILogger<AppletController> _logger;
        private readonly Dictionary<string, WnsSetting> _wnsSetting;
        public WallpaperService WallpaperService { get; }
        public IConfiguration Configuration { get; }
        public PushWnsService PushWnsService { get; }
        private readonly BlogSysContext _context;

        [ActionName("bing")]
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<WallpapersDetail>>>> GetBing()
        {
            _logger.LogInformation("sql_string:" + Configuration.GetConnectionString("OffLineNpgSqlCon"));
            List<WallpapersDetail> picModels = new List<WallpapersDetail>();
            WallpapersData wallPaperModel = await WallpaperService.GetWallparper(0, 6);
            //通过重新组装成集合给GridView
            foreach (var item in wallPaperModel.images)
            {
                picModels.Add(new WallpapersDetail()
                {
                    Title = item.copyright,
                    Source = "https://www.bing.com" + item.url
                });
            }
            return ApiResult<List<WallpapersDetail>>.Ok(picModels);
        }

        [HttpGet]
        public async Task<string> GetWeather(string city)
        {
            string urlHost = "https://www.apiopen.top/weatherApi?city=";
            string url = urlHost + city;
            HttpClient client = new HttpClient();
            string res = await client.GetStringAsync(url);
            return res;
        }

        [HttpGet]
        public async Task<string> GetCity(string location, string key)
        {
            string urlHost = "https://apis.map.qq.com/ws/geocoder/v1/?location=";
            string url = urlHost + location + "&key=" + key;
            HttpClient client = new HttpClient();
            string res = await client.GetStringAsync(url);
            return res;
        }

        /// <summary>
        /// 测试接口
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> PostChannel(string key = "yunblog")
        {
            WnsSetting wns = null;
            if (_wnsSetting != null && _wnsSetting.Count > 0 && _wnsSetting.ContainsKey(key))
            {
                wns = _wnsSetting[key];
            }
            if (wns == null)
            {
                return "error";
            }
            string uri = "https://sg2p.notify.windows.com/?token=AwYAAAC8wOyuJeZTkE%2btdaiWAxN3rs%2fscb8KWf4xV%2fD9gYwVMFgDl2yLhlXH%2fJ4x4aTIWA1MSpfLpzmslRfhuJsTgf0UkJYc6MEK5SVMRT89FA9zgoxMNqQXIqNZXo8TxN7TOAGRXUYlAYPGmJGcIKnhklBY";
            string secret = wns.Secret;
            string sid = wns.Sid;
            string notificationType = "wns/toast";
            string contentType = "text/xml";
            string content = @"
<toast>
<visual>
    <binding template='ToastGeneric'>
      <image placement='appLogoOverride' hint-crop='circle' src='https://unsplash.it/64?image=669'/>
      <text>Adam Wilson tagged you in a photo</text>
      <text>On top of McClellan Butte - with Andrew Bares</text>
      <image src='https://unsplash.it/360/202?image=883'/>
    </binding>
  </visual>
</toast>";
            string res = await PushWnsService.PostToWnsAsync(secret, sid, uri, content, notificationType, contentType);
            return res;
        }



        [HttpPost]
        [ExceptionHandle("推送错误")]
        public async Task<ActionResult<ApiResult<string>>> WnsChannel([FromBody]WnsPushArgs pushArgs)
        {
            WnsSetting wns = null;
            string content = "";
            string url = "";
            if (_wnsSetting != null && _wnsSetting.Count > 0 && pushArgs != null && _wnsSetting.ContainsKey(pushArgs.AppName))
            {
                wns = _wnsSetting[pushArgs.AppName];
                content = pushArgs.MsgContentXml;
            }
            if (wns == null)
            {
                return ApiResult<string>.Fail("参数异常");
            }
            var wnsObj= _context.WnsUrls.Where(w => w.AppName == pushArgs.AppName).OrderByDescending(w => w.UpdateDate).ToList().FirstOrDefault();
            if (wnsObj != null)
            {
                url = wnsObj.PushUrl;
            }           
            string secret = wns.Secret;
            string sid = wns.Sid;
            string notificationType = "wns/toast";
            string contentType = "text/xml";
            await PushWnsService.PostToWnsAsync(secret, sid, url, content, notificationType, contentType);
            return ApiResult<string>.Ok("推送成功");
        }


        [HttpPost]
        [ExceptionHandle("保存错误")]
        public async Task<ActionResult<ApiResult<string>>> WnsPush(WnsPushUrl wnsPushUrl)
        {
            if (wnsPushUrl == null)
            {
                return ApiResult<string>.Fail("参数错误");
            }
            bool res = _context.WnsUrls.Any(e =>e.AppName==wnsPushUrl.AppName&& e.DevFamily == wnsPushUrl.DevFamily) ? await PushWnsService.UpdateWnsPushUrl(wnsPushUrl) : await PushWnsService.InsertWnsPushUrl(wnsPushUrl);
            if (!res)
            {
                return ApiResult<string>.Fail("保存出错");
            }
            return ApiResult<string>.Ok("保存成功");
        }
    }
}