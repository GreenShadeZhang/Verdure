using System.Net.Http.Json;
using Verdure.Common;

namespace Verdure.UI.Blazor
{
    public class ArticleService
    {
        private readonly HttpClient _httpClient;
        public ArticleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Article>?> GetListAsync(QueryRequest request, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<List<Article>>("/api/Articles/GetList?PageIndex=1&PageSize=100");
        }
    }
}
