using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GreenShade.Blog.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GreenShade.Blog.Domain.Dto;
using GreenShade.Blog.Domain;
using GreenShade.Blog.Api.Services;
using GreenShade.Blog.Api.Filters;
using GreenShade.Blog.Api.Common;

namespace GreenShade.Blog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManageController : ControllerBase
    {
        private readonly BlogManageService _managecontext;

        public ManageController(BlogManageService context)
        {
            _managecontext = context;
        }
        [Authorize]
        [ActionName("arts")]
        [HttpGet]
        public async Task<ActionResult<ArticleListDto>> GetArticles(int status = 0, int pi = 1, int ps = 10)
        {
            ArticleListDto ret = null;
            List<ArticleDto> arts = new List<ArticleDto>();
            try
            {
                if (ret == null)
                {
                    ret = new ArticleListDto();
                    var artList = await _managecontext.GetArticlesByStatus(status, pi, ps);
                    artList.ForEach(art => arts.Add(new ArticleDto(art)));
                    ret.Arts = arts;
                    ret.PageTotal = await _managecontext.GetArticlesNumByStatus(status);
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
            var article = await _managecontext.GetArticle(id);

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
            await _managecontext.PostArticle(article);
            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        [Authorize]
        [ActionName("update_article")]
        [HttpPost]
        [ExceptionHandle("更新失败。")]
        public async Task<ActionResult<ApiResult<Article>>> UpdateArticle([FromBody]UpdateArtArgs art)
        {
            var article = await _managecontext.GetArticle(art.Id);
            if (article != null)
            {
                article.Title = art.BlogTitle;
                article.PicUrl = art.PicUrl;
                article.PicInfo = art.PicIntroduce;
                article.ArticleDate = DateTime.Now;
                await _managecontext.UpdateArticle(article);
                return ApiResult<Article>.Ok("更新成功。");
            }
            return ApiResult<Article>.Fail("更新失败。");

        }

        [Authorize]
        [ActionName("change_article_status")]
        [HttpPost]
        public async Task<ActionResult<Article>> ChangeArticleStatus([FromBody]ChangeArtStatusArgs art)
        {
            var article = await _managecontext.GetArticle(art.Id);
            if (article != null)
            {
                article.Status = art.Status;
                article.ArticleDate = DateTime.Now;
                await _managecontext.UpdateArticle(article);
            }
            return Ok();
        }


        [Authorize]
        [ActionName("change_article_type")]
        [HttpPost]
        public async Task<ActionResult<Article>> ChangeArticleType([FromBody]ChangeArtTypeArgs art)
        {
            var article = await _managecontext.GetArticle(art.Id);
            if (article != null)
            {
                article.Type = art.Type;
                article.ArticleDate = DateTime.Now;
                await _managecontext.UpdateArticle(article);
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Article>> DeleteArticle(string id)
        {
            var article = await _managecontext.DeleteArticle(id);
            return article;
        }

        private bool ArticleExists(string id)
        {
            return _managecontext.ArticleExists(id);
        }
        [Authorize]
        [ActionName("import_article")]
        [HttpPost]
        [ExceptionHandle("导入失败。")]
        public async Task<ActionResult<ApiResult<string>>> ImportArticle()
        {
            var file = HttpContext.Request.Form.Files["id"];
            string title = HttpContext.Request.Form["title"];
            string pic_url = HttpContext.Request.Form["pic_url"];
            string pic_info = HttpContext.Request.Form["pic_info"];
            var uploadFileBytes = new byte[file.Length];
            file.OpenReadStream().Read(uploadFileBytes, 0, (int)file.Length);
            string str = System.Text.Encoding.Default.GetString(uploadFileBytes);
            //  var dic=Directory.GetCurrentDirectory();
            //string localPath= Path.Combine(dic,file.FileName);
            //  System.IO.File.WriteAllBytes(localPath,uploadFileBytes) ;
            //Path.;
            if (!string.IsNullOrWhiteSpace(str))
            {
                Article article = new Article();
                article.Content = str;
                if (string.IsNullOrEmpty(title))
                {
                    article.Title = file.FileName.Split(".")[0];
                }
                else
                {
                    article.Title = title;
                }
                article.PicUrl = pic_url;
                article.PicInfo = pic_info;
                if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.Claims != null)
                {
                    article.UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                await _managecontext.PostArticle(article);
                return ApiResult<string>.Ok("导入成功。");
            }
            return ApiResult<string>.Fail("导入失败.");
        }
    }
}
