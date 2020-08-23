using BookmarkItCommonLibrary.Model;
using BookmarkItCommonLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Data.Handler.Contract
{
    public interface INetHandler
    {
        string AccessToken { get; set; }

        Task<string> GetRequestTokenAsync(Uri callbackUri);

        Task<string> GetAccessTokenAsync(string requestToken);

        Task<string> GetUserStats(string userName);

        Task<string> GetBookmarksAsync(string userName, long since = 0, string searchKey = default, int? count = default, int? offset = default,
            BookmarkFilter filter = BookmarkFilter.All, bool? isFavorite = default, string tag = default,
            BookmarkType bookmarkType = BookmarkType.Unknown, SortBy sortBy = SortBy.Newest,
            BookmarkDetailDepth detailDepth = BookmarkDetailDepth.Complete, string domain = default);

        Task<IList<string>> GetPaginatedArticleAsync(string url);

        Task<string> AddBookmarkAsync(string userName, string url);

        Task<string> AddBookmarksAsync(string userName, IEnumerable<BookmarkBObj> bookmarks);
    }
}
