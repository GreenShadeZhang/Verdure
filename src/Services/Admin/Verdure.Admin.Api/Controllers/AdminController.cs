using Microsoft.AspNetCore.Mvc;
using Verdure.Admin.Core;
using Verdure.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Verdure.Admin.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;

        private readonly IAdminService _service;

        public AdminController(ILogger<AdminController> logger, IAdminService service)
        {
            _logger = logger;

            _service = service;
        }

        [HttpPost]
        public Task<Article> AddAsync(Article article)
        {
            return _service.AddAsync(article, CancellationToken.None);
        }

        [HttpPost]
        public Task<Article> UpdateAsync(Article article)
        {
            return _service.UpdateAsync(article, CancellationToken.None);
        }

        [HttpDelete]
        public Task<bool> DeleteAsync(string id)
        {
            return _service.DeleteAsync(id, CancellationToken.None);
        }

        [HttpPost]
        public Task<Article> ImportArticleAsync()
        {
            return _service.ImportArticleAsync(CancellationToken.None);
        }
    }
}
