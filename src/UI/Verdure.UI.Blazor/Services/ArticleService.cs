using System.Net.Http.Json;
using Verdure.Common;

namespace Verdure.UI.Blazor
{
    public class ArticleService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public ArticleService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<List<Article>?> GetListAsync(QueryRequest request, CancellationToken cancellationToken = default)
        {
            var url = $"{_configuration["AdminPrefix"]}/api/Articles/GetList?PageIndex=1&PageSize=100";

            return await _httpClient.GetFromJsonAsync<List<Article>>(url);
        }
    }
}
