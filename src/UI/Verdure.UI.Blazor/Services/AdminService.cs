using System.Net;

namespace Verdure.UI.Blazor
{
    public class AdminService
    {
        private readonly HttpClient _httpClient;
        public AdminService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> AddAsync(HttpContent? content, CancellationToken cancellationToken = default)
        {
            var ret = await _httpClient.PostAsync("/api/Admin/ImportArticle", content);

            if (ret != null && ret.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}
