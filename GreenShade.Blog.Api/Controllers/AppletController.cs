using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GreenShade.Blog.Api.Common;
using GreenShade.Blog.DataAccess.Services;
using GreenShade.Blog.Domain.Dto;
using GreenShade.Blog.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
             IOptions<Dictionary<string,WnsSetting>> wnsSettingsOptions)
        {
            PushWnsService = pushWnsService;
            Configuration = configuration;
            WallpaperService = wallpaperService;
            _wnsSetting = wnsSettingsOptions.Value;
        }
        private readonly Dictionary<string, WnsSetting> _wnsSetting;
        public WallpaperService WallpaperService { get; }
        public IConfiguration Configuration { get; }
        public PushWnsService PushWnsService { get; }

        [ActionName("bing")]
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<WallpapersDetail>>>> GetBing()
        {
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
        [HttpGet]
        public async Task<string> PostChannel(string key = "yunblog")
        {
            WnsSetting wns = null;
            if(_wnsSetting!=null&& _wnsSetting.Count>0&&_wnsSetting.ContainsKey(key))
            {
                wns = _wnsSetting[key];
            }
            if (wns == null)
            {
                return "error";
            }
            string uri = "https://sg2p.notify.windows.com/?token=AwYAAAC8wOyuJeZTkE%2btdaiWAxN3rs%2fscb8KWf4xV%2fD9gYwVMFgDl2yLhlXH%2fJ4x4aTIWA1MSpfLpzmslRfhuJsTgf0UkJYc6MEK5SVMRT89FA9zgoxMNqQXIqNZXo8TxN7TOAGRXUYlAYPGmJGcIKnhklBY";
            string secret =wns.Secret;
            string sid = wns.Sid;
            string notificationType = "wns/toast";
            string contentType = "text/xml";
            string content =@"
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
            string res =await PushWnsService.PostToWnsAsync(secret, sid, uri, content, notificationType, contentType);
            return res;
        }      
    }
}