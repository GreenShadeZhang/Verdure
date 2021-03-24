using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Verdure.ApplicationCore;

namespace Verdure.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        private readonly IBlogManageService _blogManageService;

        public ArticlesController(IArticleService articleService, IBlogManageService blogManageService)
        {
            _articleService = articleService;
            _blogManageService = blogManageService;
        }

        [HttpGet]
        public async Task<ActionResult> GetsAsync(int pi = 1, int ps = 10)
        {
            var list = await _articleService.ListAsync(pi, ps, CancellationToken.None);

            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Article article)
        {
            await _articleService.AddAsync(article, CancellationToken.None);

            return Ok();
        }

        [HttpPost("import_article")]
        public async Task<ActionResult> ImportArticleAsync()
        {
            var ret = await _blogManageService.ImportArticleAsync(CancellationToken.None);

            return Ok(ret);
        }
    }
}
