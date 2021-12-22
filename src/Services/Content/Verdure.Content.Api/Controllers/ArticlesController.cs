using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Verdure.Common;
using Verdure.Core;

namespace Verdure.Content.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ILogger<ArticlesController> _logger;

        private readonly IArticleService _articleService;

        public ArticlesController(ILogger<ArticlesController> logger, IArticleService articleService)
        {
            _logger = logger;

            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] QueryRequest queryRequest)
        {
            var ret = await _articleService.GetListAsync(queryRequest, CancellationToken.None);

            return this.Ok(ret);
        }

        [HttpPost]
        public Task<Article> AddAsync(Article article)
        {
            return _articleService.AddAsync(article, CancellationToken.None);
        }


        [HttpGet]
        public Task<Article> GetAsync(string id)
        {
            return _articleService.GetAsync(id, CancellationToken.None);
        }

        [Authorize]
        [HttpPost]
        public Task<Article> ImportArticleAsync()
        {
            return _articleService.ImportArticleAsync(CancellationToken.None);
        }
    }
}
