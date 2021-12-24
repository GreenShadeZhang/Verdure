using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Verdure.UI.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<JwtAuthorizationMessageHandler>();

// AddHttpClient is an extension in Microsoft.Http.Extensions
builder.Services.AddHttpClient("WebAPI",
        client => client.BaseAddress = new Uri(builder.Configuration.GetSection("BaseUrl").Value))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetSection("BaseUrl").Value) });

builder.Services.AddHttpClient<ArticleService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetSection("ArticleBaseUrl").Value);
});

builder.Services.AddHttpClient<AdminService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetSection("AdminBaseUrl").Value);
});

builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();
builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
    options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");
});

await builder.Build().RunAsync();
