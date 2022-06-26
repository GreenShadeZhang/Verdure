using System.Net;

namespace Verdure.UI.Blazor
{
    public class AdminService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public AdminService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<bool> AddAsync(HttpContent? content, CancellationToken cancellationToken = default)
        {
            var url = $"{_configuration["AdminPrefix"]}/api/Admin/ImportArticle";

            var ret = await _httpClient.PostAsync(url, content);

            if (ret != null && ret.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}
