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
        public AppletController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // GET: api/Bing
        [HttpGet]
        public async Task<ActionResult> GetBing()
        {
            List<WallpapersDetail> picModels = new List<WallpapersDetail>();
            WallpaperService wallpaperService = new WallpaperService();
            WallpapersData wallPaperModel = await wallpaperService.GetWallparper(0, 6);
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


        //// PUT: api/Bing/5
        //[HttpGet]
        //public void PostChannel()
        //{
        //    string sid = "ms-app://s-1-15-2-73882458-3227807263-4067040658-804817547-1359916503-4130225270-1080480494";
        //    string sre = "yzs1CsGxtWzNH323PO7ieLdduBJ/3OX6";
        //    string xmlt = "";
        //    string channel = Configuration.GetConnectionString("UrlChannel");
        //    string res = PushWnsService.PostToWns(sre, sid, channel, xmlt, "wns/toast", "text/xml");
        //}
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}