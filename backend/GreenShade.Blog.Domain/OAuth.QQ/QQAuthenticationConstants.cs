namespace GreenShade.Blog.Domain.OAuth
{ /// <summary>
  /// Contains constants specific to the <see cref="QQAuthenticationHandler"/>.
  /// </summary>
    public static class QQAuthenticationConstants
    {
        public static class Claims
        {
            public const string AvatarFullUrl = "urn:qq:avatar_full";
            public const string AvatarUrl = "urn:qq:avatar";
            public const string PictureFullUrl = "urn:qq:picture_full";
            public const string PictureMediumUrl = "urn:qq:picture_medium";
            public const string PictureUrl = "urn:qq:picture";
        }
    }
}
