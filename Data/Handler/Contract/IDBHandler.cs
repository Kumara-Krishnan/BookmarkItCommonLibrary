using BookmarkItCommonLibrary.Model;
using BookmarkItCommonLibrary.Model.Entity;
using BookmarkItCommonLibrary.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Adapter.DB;
using Utilities.Model;

namespace BookmarkItCommonLibrary.Data.Handler.Contract
{
    public interface IDBHandler
    {
        void Initialize(string fileName, string path, SQLiteOpenFlags openFlags = SQLiteConnectionMode.ReadWrite);

        UserDetails GetUserDetails(string userId);

        int AddOrReplaceUserDetails(UserDetails user);

        T GetSetting<T>(string userId, string key) where T : SettingsBase, new();

        int SetSetting<T>(T setting) where T : SettingsBase, new();

        int AddOrReplaceParsedBookmarksResponse(ParsedBookmarksResponse bookmarksResponse, bool updateLastSyncedTime = false);

        IEnumerable<BookmarkBObj> GetBookmarks(string userId, BookmarkFilter filter, bool? isFavorite, string tag, BookmarkType bookmarkType,
            SortBy sortBy, int? count, int? offset);

        IEnumerable<BookmarkBObj> GetArticlesToBeFetched(string userId, int limit = CommonConstants.ArticlesFetchLimit);

        IEnumerable<ImageBObj> GetImagesByBookmarkIds(params string[] bookmarkIds);

        IEnumerable<VideoBObj> GetVideosByBookmarkIds(params string[] bookmarkIds);

        int AddOrReplaceArticles(IEnumerable<Article> articles);

        int UpdateArticleError(string bookmarkId);
    }
}
