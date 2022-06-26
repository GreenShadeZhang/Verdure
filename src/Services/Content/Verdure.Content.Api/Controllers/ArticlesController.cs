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

        private readonly IArticleService _service;

        public ArticlesController(ILogger<ArticlesController> logger, IArticleService service)
        {
            _logger = logger;

            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] QueryRequest queryRequest)
        {
            var ret = await _service.GetListAsync(queryRequest, CancellationToken.None);

            return this.Ok(ret);
        }

        [HttpGet]
        public Task<Article> GetAsync(string id)
        {
            return _service.GetAsync(id, CancellationToken.None);
        }
    }
}
