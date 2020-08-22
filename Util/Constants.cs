using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Util
{
    public static class CommonConstants
    {
        public const string DBFileName = "Bookmark-It.db";
        public const string AppServiceName = "SystemTrayExtensionService";
        public const string PocketConsumerKey = "89366-ce9ed95c6f03b291a9052320";

        public const string PocketBaseUrl = "https://getpocket.com/";
        public const string PocketV3BaseUrl = PocketBaseUrl + "v3/";

        public const string PocketRequestTokenUrl = PocketV3BaseUrl + "oauth/request";
        public const string PocketAuthorizationUrl = PocketBaseUrl + "auth/authorize";
        public const string PocketAccessTokenUrl = PocketV3BaseUrl + "oauth/authorize";
        public const string PocketLogoutUrl = PocketBaseUrl + "lo";

        public const string ConsumerKeyParamName = "consumer_key";
        public const string RedirectUriParamName = "redirect_uri";
        public const string WebAuthenticationBrokerParamName = "webauthenticationbroker";
        public const string ForceParamName = "force";
        public const string RequestTokenParamName = "request_token";
        public const string LoginParamValue = "login";
        public const string SignUpParamValue = "signup";

        public const string AppSetting = nameof(AppSetting);

        public const int BookmarksFetchLimit = 100;
        public const int ArticlesFetchLimit = 10;
    }

    public static class SettingKeys
    {
        public const string CurrentUser = nameof(CurrentUser);
    }

    public static class JsonResponseKeys
    {
        public const string RequestToken = "code";
        public const string AccessToken = "access_token";
        public const string UserName = "username";
        public const string Account = "account";
        public const string PocketUserId = "user_id";
        public const string Email = "email";
        public const string FirstName = "first_name";
        public const string LastName = "last_name";
        public const string CreationTime = "birth";
        public const string Profile = "profile";
        public const string DisplayName = "name";
        public const string Description = "description";
        public const string AvatarUrl = "avatar_url";
        public const string UpdatedTime = "updated_at";
    }

    public static class CredentialResourceKeys
    {
        public const string RequestToken = nameof(RequestToken);
        public const string AccessToken = nameof(AccessToken);
    }
}
