using Microsoft.AspNetCore.Mvc;
using Verdure.Core;

namespace Verdure.Api.Controllers
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

        [HttpGet]
        public Task<Article> GetAsync(string id)
        {
            return _articleService.GetAsync(id, CancellationToken.None);
        }

        [HttpGet]
        public Task<Article> ImportArticleAsync()
        {
            return _articleService.ImportArticleAsync(CancellationToken.None);
        }
    }
}
