using BookmarkItCommonLibrary.Data.Contract;
using BookmarkItCommonLibrary.Data.Parser;
using BookmarkItCommonLibrary.Domain;
using BookmarkItCommonLibrary.Model;
using BookmarkItCommonLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Extension;
using Utilities.UseCase;

namespace BookmarkItCommonLibrary.Data
{
    public class GetBookmarksDataManager : DataManagerBase, IGetBookmarksDataManager
    {
        public async Task GetBookmarksAsync(GetBookmarksRequest request, ICallback<GetBookmarksResponse> callback = null)
        {
            if (request.Type.HasLocalStorage())
            {
                try
                {
                    var bookmarksFromDB = GetBookmarksFromDB(request);
                    callback.OnSuccessOrFailed(ResponseType.LocalStorage, new GetBookmarksResponse(bookmarksFromDB), IsValidResponse);
                }
                catch (Exception)
                {

                }
            }

            if (request.Type.HasNetwork())
            {
                var bookmarksFromServer = await FetchBookmarksFromServerAsync(request).ConfigureAwait(false);
                callback.OnSuccessOrFailed(ResponseType.Network, new GetBookmarksResponse(bookmarksFromServer), IsValidResponse);
            }
        }

        protected IEnumerable<BookmarkBObj> GetBookmarksFromDB(GetBookmarksRequest request)
        {
            return DBHandler.GetBookmarks(request.UserId, request.Filter, request.IsFavorite, request.Tag, request.BookmarkType, request.SortBy,
               request.Count, request.Offset);
        }

        protected async Task<IEnumerable<BookmarkBObj>> FetchBookmarksFromServerAsync(GetBookmarksRequest request)
        {
            var parsedBookmarksResponse = await FetchParsedBookmarksResponseFromServerAsync(request).ConfigureAwait(false);
            return parsedBookmarksResponse.Bookmarks.Where(b => b.Status != BookmarkStatus.Deleted);
        }

        protected async Task<ParsedBookmarksResponse> FetchParsedBookmarksResponseFromServerAsync(GetBookmarksRequest request)
        {
            var response = await NetHandler.GetBookmarksAsync(request.UserId, request.Since, request.SearchKey,
                request.Count, request.Offset, request.Filter, request.IsFavorite, request.Tag, request.BookmarkType,
                request.SortBy, domain: request.Domain).ConfigureAwait(false);
            var parsedResponse = ResponseDataParser.ParseBookmarks(request.UserId, response);
            DBHandler.AddOrReplaceParsedBookmarksResponse(parsedResponse, updateLastSyncedTime: RequestType.Sync == request.Type);
            return parsedResponse;
        }

        protected bool IsValidResponse(GetBookmarksResponse response)
        {
            return response.Bookmarks.IsNonEmpty();
        }
    }
}
