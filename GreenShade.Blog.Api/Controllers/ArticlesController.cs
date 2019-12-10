using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenShade.Blog.DataAccess.Data;
using GreenShade.Blog.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GreenShade.Blog.DataAccess.Services;
using GreenShade.Blog.Domain.Dto;
using System.IO;

namespace GreenShade.Blog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService _context;

        public ArticlesController(ArticleService context)
        {
            _context = context;
        }
        [ActionName("arts")]
        [HttpGet]
        public async Task<ActionResult<ArticleListDto>> GetArticles(int pi = 1, int ps = 10)
        {
            ArticleListDto ret = null;
            List<ArticleDto> arts = new List<ArticleDto>();
            try
            {
                if (ret == null)
                {
                    ret = new ArticleListDto();
                    var artList = await _context.GetArticles(pi, ps);
                    artList.ForEach(art => arts.Add(new ArticleDto(art)));
                    ret.Arts = arts;
                    ret.PageTotal =await _context.GetArticlesNum();
                }


                return Ok(ret);
            }
            catch (Exception ex)
            {
                //return BadRequest();
            }
            return ret;
        }

        [ActionName("article_detail")]
        [HttpGet]
        public async Task<ActionResult<ArticleDto>> GetArticle(string id)
        {
            var user = HttpContext.User;
            var article = await _context.GetArticle(id);

            if (article == null)
            {
                return NotFound();
            }
            var ret = new ArticleDto(article);

            return ret;
        }


        [Authorize]
        [ActionName("create_article")]
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle([FromForm]Article article)
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.Claims != null)
            {
                foreach (var item in HttpContext.User.Claims)
                {
                    if (item.Type == ClaimTypes.NameIdentifier)
                    {
                        article.UserId = item.Value;
                    }
                }
            }
            await _context.PostArticle(article);
            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Article>> DeleteArticle(string id)
        {
            var article = await _context.DeleteArticle(id);
            return article;
        }

        private bool ArticleExists(string id)
        {
            return _context.ArticleExists(id);
        }
        [Authorize]
        [ActionName("import_article")]
        [HttpPost]
        public async Task<ActionResult> ImportArticle()
        {
            var file = HttpContext.Request.Form.Files["id"];
            var uploadFileBytes = new byte[file.Length];
            file.OpenReadStream().Read(uploadFileBytes, 0, (int)file.Length);
          string str= System.Text.Encoding.Default.GetString(uploadFileBytes);
          //  var dic=Directory.GetCurrentDirectory();
          //string localPath= Path.Combine(dic,file.FileName);
          //  System.IO.File.WriteAllBytes(localPath,uploadFileBytes) ;
            //Path.;
            if (!string.IsNullOrWhiteSpace(str))
            {
                Article article = new Article();
                article.Content = str;
                article.Title = file.FileName.Split(".")[0];
                if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.Claims != null)
                {
                    foreach (var item in HttpContext.User.Claims)
                    {
                        if (item.Type == ClaimTypes.NameIdentifier)
                        {
                            article.UserId = item.Value;
                        }
                    }
                }
                await _context.PostArticle(article);
                return new JsonResult(new { msg = "导入成功",code=0});
            }
            else
            {
                return new JsonResult(new { msg = "导入失败" ,code=-1});
            }
           
            
        }
    }
}
