using BookmarkItCommonLibrary.Data.Handler.Contract;
using BookmarkItCommonLibrary.Model;
using BookmarkItCommonLibrary.Util;
using Newtonsoft.Json;
using ReadSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Utilities.Adapter.Net.Contract;
using Utilities.Error;
using Utilities.Extension;

namespace BookmarkItCommonLibrary.Data.Handler
{
    public sealed class NetHandler : INetHandler
    {
        private readonly INetAdapter NetAdapter;

        private readonly Reader ArticleReader;

        public string AccessToken { get; set; }

        public NetHandler(INetAdapter netAdapter)
        {
            NetAdapter = netAdapter;
            ArticleReader = new Reader();
        }

        public Task<string> GetRequestTokenAsync(Uri callbackUri)
        {
            ThrowIfNetworkUnavailable();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(CommonConstants.PocketRequestTokenUrl));
            requestMessage.Headers.Add("X-Accept", "application/json");
            requestMessage.Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(CommonConstants.ConsumerKeyParamName,CommonConstants.PocketConsumerKey),
                new KeyValuePair<string, string>(CommonConstants.RedirectUriParamName, callbackUri.ToString())
            });
            return NetAdapter.SendAsync(requestMessage);
        }

        public Task<string> GetAccessTokenAsync(string requestToken)
        {
            ThrowIfNetworkUnavailable();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(CommonConstants.PocketAccessTokenUrl));
            requestMessage.Headers.Add("X-Accept", "application/json");
            requestMessage.Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string,string>(CommonConstants.ConsumerKeyParamName,CommonConstants.PocketConsumerKey),
                new KeyValuePair<string, string>("code", requestToken),
                new KeyValuePair<string, string>("account", "1")
            });

            return NetAdapter.SendAsync(requestMessage);
        }

        public Task<string> GetUserStats(string userName)
        {
            ThrowIfNetworkUnavailable();
            var requestParams = new List<KeyValuePair<string, string>>();
            AddAuthorizationParameters(userName, requestParams);
            return NetAdapter.PostAsync(GetAbsoluteRequestUrl("stats"), requestParams);
        }

        public Task<string> GetBookmarksAsync(string userName, long since = 0, string searchKey = default, int? count = default, int? offset = default,
            BookmarkFilter filter = BookmarkFilter.All, bool? isFavorite = default, string tag = default,
            BookmarkType bookmarkType = BookmarkType.Unknown, SortBy sortBy = SortBy.Newest,
            BookmarkDetailDepth detailDepth = BookmarkDetailDepth.Complete, string domain = default)
        {
            ThrowIfNetworkUnavailable();
            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("state", filter.ToStringLower()),
                new KeyValuePair<string, string>("sort", sortBy.ToStringLower()),
                new KeyValuePair<string, string>("detailType", detailDepth.ToStringLower()),
            };
            if (isFavorite != default)
            {
                requestParams.Add(new KeyValuePair<string, string>("favorite", isFavorite.GetInt().ToString()));
            }
            if (!string.IsNullOrEmpty(tag))
            {
                requestParams.Add(new KeyValuePair<string, string>("tag", tag));
            }
            if (bookmarkType != BookmarkType.Unknown)
            {
                requestParams.Add(new KeyValuePair<string, string>("contentType", bookmarkType.ToStringLower()));
            }
            if (!string.IsNullOrEmpty(searchKey))
            {
                requestParams.Add(new KeyValuePair<string, string>("search", searchKey));
            }
            if (!string.IsNullOrEmpty(domain))
            {
                requestParams.Add(new KeyValuePair<string, string>("domain", domain));
            }
            if (since > 0)
            {
                requestParams.Add(new KeyValuePair<string, string>("since", since.ToString()));
            }
            if (count != default)
            {
                requestParams.Add(new KeyValuePair<string, string>("count", count.ToString()));
            }
            if (offset != default)
            {
                requestParams.Add(new KeyValuePair<string, string>("offset", offset.ToString()));
            }

            AddAuthorizationParameters(userName, requestParams);
            return NetAdapter.PostAsync(GetAbsoluteRequestUrl("get"), requestParams);
        }

        public async Task<IList<string>> GetPaginatedArticleAsync(string url)
        {
            var pages = new List<string>();
            Article article;
            do
            {
                article = await ArticleReader.Read(new Uri(url, UriKind.RelativeOrAbsolute)).ConfigureAwait(false);
                pages.Add(article.Content);
                url = article.NextPage?.AbsolutePath;
            }
            while (article != default && url != default);

            return pages;
        }

        public Task<string> AddBookmarkAsync(string userName, string url)
        {
            ThrowIfNetworkUnavailable();
            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("url",url),
            };
            AddAuthorizationParameters(userName, requestParams);
            return NetAdapter.PostAsync(GetAbsoluteRequestUrl("add"), requestParams);
        }

        public Task<string> AddBookmarksAsync(string userName, IEnumerable<BookmarkBObj> bookmarks)
        {
            ThrowIfNetworkUnavailable();
            JArray jAddActions = new JArray();
            foreach (var bookmark in bookmarks)
            {
                var jAddAction = new JObject
                {
                    ["action"] = "add",
                    ["url"] = bookmark.Url
                };
                if (bookmark.CreatedTime > 0)
                {
                    jAddAction["time"] = bookmark.CreatedTime;
                }
                if (!string.IsNullOrEmpty(bookmark.Title))
                {
                    jAddAction["title"] = bookmark.Title;
                }
                if (bookmark.Tags.IsNonEmpty())
                {
                    jAddAction["tags"] = string.Join(",", bookmark.Tags.Select(t => t.Name));
                }
                jAddActions.Add(jAddAction);
            }
            return SendBatchRequestAsync(userName, jAddActions);
        }

        private Task<string> SendBatchRequestAsync(string userName, JArray jActions)
        {
            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("actions", jActions.ToString(Formatting.None))
            };
            AddAuthorizationParameters(userName, requestParams);
            return NetAdapter.PostAsync(GetAbsoluteRequestUrl("send"), requestParams);
        }

        private string GetAbsoluteRequestUrl(string method)
        {
            return CommonConstants.PocketV3BaseUrl + method;
        }

        private void AddAuthorizationParameters(string userName, List<KeyValuePair<string, string>> requestParamters)
        {
            requestParamters.Add(GetAccessTokenParameter(userName));
            requestParamters.Add(GetConsumerKeyParamter());
        }

        private KeyValuePair<string, string> GetAccessTokenParameter(string userName)
        {
            return new KeyValuePair<string, string>("access_token", AccessToken);
            //CredentialLocker.RetrievePassword(CredentialResourceKeys.AccessToken, userName)
        }

        private KeyValuePair<string, string> GetConsumerKeyParamter()
        {
            return new KeyValuePair<string, string>(CommonConstants.ConsumerKeyParamName, CommonConstants.PocketConsumerKey);
        }

        private void ThrowIfNetworkUnavailable()
        {
            //TODO: Doesn't take into account of any virutal machine connections resulting in true in that scenario.
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                throw new NoInternetAccessException();
            }
        }
    }
}
