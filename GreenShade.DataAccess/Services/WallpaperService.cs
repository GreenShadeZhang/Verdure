using GreenShade.Blog.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GreenShade.Blog.DataAccess.Services
{
    public class WallpaperService
    {
        public async Task<WallpapersData> GetWallparper(int index, int number)
        {
            // string url = "https://cn.bing.com/HPImageArchive.aspx?format=js&idx=8&n=25";
            string url = string.Format("https://cn.bing.com/HPImageArchive.aspx?format=js&idx={0}&n={1}&mkt=zh-cn", index, number);
            Uri uri = new Uri(url);
            var httpClient = new HttpClient();
            string json = await httpClient.GetStringAsync(uri);
            WallpapersData wallPapersData = JsonConvert.DeserializeObject<WallpapersData>(json);
            return wallPapersData;
        }
    }
}
