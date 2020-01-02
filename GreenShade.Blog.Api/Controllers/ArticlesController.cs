using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GreenShade.Blog.DataAccess.Services;
using GreenShade.Blog.Domain.Dto;

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
        public async Task<ActionResult<ArticleListDto>> GetArticles(int type=0,int pi = 1, int ps = 10)
        {
            ArticleListDto ret = null;
            List<ArticleDto> arts = new List<ArticleDto>();
            try
            {
                if (ret == null)
                {
                    ret = new ArticleListDto();
                    var artList = await _context.GetArticlesByType(type,pi, ps);
                    artList.ForEach(art => arts.Add(new ArticleDto(art)));
                    ret.Arts = arts;
                    ret.PageTotal =await _context.GetArticlesNumByType(type);
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
    }
}
