using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Verdure.UI.Blazor
{
    public class JwtAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public JwtAuthorizationMessageHandler(IAccessTokenProvider provider,
          NavigationManager navigationManager,
          IConfiguration configuration)
          : base(provider, navigationManager)
        {
            var url = $"{configuration.GetSection("BaseUrl").Value}/api/Articles/ImportArticle";

            ConfigureHandler(
                authorizedUrls: new[] { url }
                );

        }
    }
}
