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
    //[Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService _context;

        public ArticlesController(ArticleService context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet("api/Articles")]
        public async Task<ActionResult<List<ArticleDto>>> GetArticles(int pi = 1, int ps = 10)
        {
            List<ArticleDto> ret = null;
            try
            {
                if (ret == null)
                {
                    ret = new List<ArticleDto>();
                    var artList = await _context.GetArticles(pi, ps);
                    artList.ForEach(art => ret.Add(new ArticleDto(art)));
                }


                return Ok(ret);
            }
            catch (Exception ex)
            {
                //return BadRequest();
            }
            return ret;
        }

        // GET: api/Articles/5
        [HttpGet("api/Articles/{id}")]
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


        // POST: api/Articles
        [Authorize]
        [HttpPost("api/Articles")]
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

        // DELETE: api/Articles/5
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
        [HttpPost("api/Articles/ImportArticle")]
        public async Task<ActionResult<Article>> ImportArticle()
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
                return CreatedAtAction("GetArticle", new { id = article.Id }, article);
            }
            else
            {
                return new JsonResult(new { msg = "导入失败" });
            }
           
            
        }
    }
}
