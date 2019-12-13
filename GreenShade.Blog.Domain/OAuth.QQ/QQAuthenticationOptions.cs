using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static GreenShade.Blog.Domain.OAuth.QQAuthenticationConstants;
namespace GreenShade.Blog.Domain.OAuth
{
    /// <summary>
    /// Defines a set of options used by <see cref="QQAuthenticationHandler"/>.
    /// </summary>
    public class QQAuthenticationOptions : OAuthOptions
    {
        public QQAuthenticationOptions()
        {
            ClaimsIssuer = QQAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(QQAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = QQAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = QQAuthenticationDefaults.TokenEndpoint;
            UserIdentificationEndpoint = QQAuthenticationDefaults.UserIdentificationEndpoint;
            UserInformationEndpoint = QQAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("get_user_info");

            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(Claims.PictureUrl, "figureurl");
            ClaimActions.MapJsonKey(Claims.PictureMediumUrl, "figureurl_1");
            ClaimActions.MapJsonKey(Claims.PictureFullUrl, "figureurl_2");
            ClaimActions.MapJsonKey(Claims.AvatarUrl, "figureurl_qq_1");
            ClaimActions.MapJsonKey(Claims.AvatarFullUrl, "figureurl_qq_2");           
        }

        /// <summary>
        /// Gets or sets the URL of the user identification endpoint (aka "OpenID endpoint").
        /// </summary>
        public string UserIdentificationEndpoint { get; set; }
    }
}
