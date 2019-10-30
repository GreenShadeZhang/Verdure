using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GreenShade.Blog.DataAccess.Services;
using GreenShade.Blog.Domain.Dto;
using GreenShade.Blog.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GreenShade.Blog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppletController : ControllerBase
    {
        public AppletController(IConfiguration configuration, PushWnsService pushWnsService, WallpaperService wallpaperService)
        {
            PushWnsService = pushWnsService;
            Configuration = configuration;
            WallpaperService = wallpaperService;
        }
        public WallpaperService WallpaperService { get; }
        public IConfiguration Configuration { get; }
        public PushWnsService PushWnsService { get; }
        // GET: api/Bing
        [HttpGet]
        public async Task<ActionResult> GetBing()
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
            return new JsonResult(picModels);
        }

        // GET: api/Bing/5
        [HttpGet]
        public async Task<string> GetWeather(string city)
        {
            string urlHost = "https://www.apiopen.top/weatherApi?city=";
            string url = urlHost + city;
            HttpClient client = new HttpClient();
            string res = await client.GetStringAsync(url);
            return res;
        }

        // GET: api/Bing/5
        [HttpGet]
        public async Task<string> GetCity(string location, string key)
        {
            string urlHost = "https://apis.map.qq.com/ws/geocoder/v1/?location=";
            string url = urlHost + location + "&key=" + key;
            HttpClient client = new HttpClient();
            string res = await client.GetStringAsync(url);
            return res;
        }

        // PUT: api/Bing/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }



        [HttpGet]
        public string PostChannel()
        {
            string uri = "https://sg2p.notify.windows.com/?token=AwYAAADQCl9EgOdsNvLepNi8wZchyME9K0uTnM8NsEa0xdXj23E0ez1fCVoD0OiFUJiyUhxSEO%2b33J%2bV7ns%2fEJEYOuga%2fYnjfWAgjpLqJYW7h0HVDybrUmniAisYH5yR9jr2bSrcCmv%2f7nbMXTlIaGbc2mGb";
            string secret = "secret";
            string sid = "sid";
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
            string res = PushWnsService.PostToWns(secret, sid, uri, content, notificationType, contentType);
            return res;
        }
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}