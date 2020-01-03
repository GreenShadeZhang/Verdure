using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GreenShade.Blog.DataAccess.Services;
using GreenShade.Blog.Domain.Dto;
using GreenShade.Blog.Api.Filters;
using GreenShade.Blog.Api.Common;
using Microsoft.Extensions.Logging;

namespace GreenShade.Blog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private ILogger<ArticlesController> _logger;
        private readonly ArticleService _context;
        public ArticlesController(ArticleService context,
            ILogger<ArticlesController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [ActionName("arts")]
        [HttpGet]
        [ExceptionHandle("获取博客失败请重试")]
        public async Task<ActionResult<ApiResult<ArticleListDto>>> GetArticles(int type=0,int pi = 1, int ps = 10)
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
                return ApiResult<ArticleListDto>.Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError("获取博客失败",ex);
            }           
            return ApiResult<ArticleListDto>.Ok(ret);
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
