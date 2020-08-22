using BookmarkItCommonLibrary.DI;
using BookmarkItCommonLibrary.Model;
using BookmarkItCommonLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities.UseCase;

namespace BookmarkItCommonLibrary.Domain
{
    internal interface IGetBookmarksDataManager
    {
        Task GetBookmarksAsync(GetBookmarksRequest request, ICallback<GetBookmarksResponse> callback = default);
    }

    public class GetBookmarksRequest : AuthenticatedUseCaseRequest
    {
        public BookmarkFilter Filter { get; set; } = BookmarkFilter.All;
        public bool? IsFavorite { get; set; }
        public string Tag { get; set; }
        public BookmarkType BookmarkType { get; set; }
        public SortBy SortBy { get; set; }
        public string SearchKey { get; set; }
        public string Domain { get; set; }
        public long Since { get; set; }
        public int? Count { get; set; }
        public int? Offset { get; set; }
        public GetBookmarksRequest(RequestType type, string userId, CancellationTokenSource cts = default) : base(type, userId, cts) { }

        public GetBookmarksRequest(RequestType type, long since, string userId, CancellationTokenSource cts = default) : this(type, userId, cts)
        {
            Since = since;
        }
    }

    public class GetBookmarksResponse
    {
        public readonly List<BookmarkBObj> Bookmarks = new List<BookmarkBObj>();

        public GetBookmarksResponse(IEnumerable<BookmarkBObj> bookmarks)
        {
            Bookmarks.AddRange(bookmarks);
        }
    }

    public interface IGetBookmarksPresenterCallback : ICallback<GetBookmarksResponse> { }

    public sealed class GetBookmarks : UseCaseBase<GetBookmarksRequest, GetBookmarksResponse>
    {
        private readonly IGetBookmarksDataManager DataManager;

        public GetBookmarks(GetBookmarksRequest request, IGetBookmarksPresenterCallback callback) : base(request, callback)
        {
            DataManager = CommonDIServiceProvider.Instance.GetService<IGetBookmarksDataManager>();
        }

        protected override Task Action()
        {
            return DataManager.GetBookmarksAsync(Request, new UseCaseCallback(this));
        }

        class UseCaseCallback : CallbackBase<GetBookmarksResponse>
        {
            private readonly GetBookmarks UseCase;

            public UseCaseCallback(GetBookmarks useCase)
            {
                UseCase = useCase;
            }

            public override void OnError(UseCaseError error)
            {
                UseCase.PresenterCallback?.OnError(error);
            }

            public override void OnFailed(IUseCaseResponse<GetBookmarksResponse> response)
            {
                UseCase.PresenterCallback?.OnFailed(response);
            }

            public override void OnSuccess(IUseCaseResponse<GetBookmarksResponse> response)
            {
                UseCase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
}
