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

namespace GreenShade.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService _context;

        public ArticlesController(ArticleService context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<List<ArticleDto>>> GetArticles(int pi=1,int ps=10)
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

            }
            return BadRequest();
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
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

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Article>> DeleteArticle(string id)
        {
          var article= await _context.DeleteArticle(id);
            return article;
        }

        private bool ArticleExists(string id)
        {
            return _context.ArticleExists(id);
        }
    }
}
