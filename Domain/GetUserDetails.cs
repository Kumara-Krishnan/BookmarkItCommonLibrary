using BookmarkItCommonLibrary.DI;
using BookmarkItCommonLibrary.Model;
using BookmarkItCommonLibrary.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities.UseCase;

namespace BookmarkItCommonLibrary.Domain
{
    public interface IGetUserDetailsDataManager
    {
        Task GetUserDetailsAsync(GetUserDetailsRequest request, ICallback<GetUserDetailsResponse> callback);

        UserDetails GetUserDetailsFromDB(GetUserDetailsRequest request);

        Task<ParsedUserDetails> FetchUserDetailsFromServerAsync(GetUserDetailsRequest request, UserDetails userDetailsFromDB = default);

        Task<UserStat> FetchUserStatFromServerAsync(string userId);
    }

    public class GetUserDetailsRequest : AuthenticatedUseCaseRequest
    {
        public readonly string RequestToken;
        public readonly bool SetAsCurrentUser;

        public GetUserDetailsRequest(RequestType type, string userId, string requestToken = default, bool setAsCurrentUser = false,
            CancellationTokenSource cts = default) : base(type, userId, cts)
        {
            RequestToken = requestToken;
            SetAsCurrentUser = setAsCurrentUser;
        }
    }

    public class GetUserDetailsResponse
    {
        public readonly UserDetails User;

        public GetUserDetailsResponse(UserDetails user)
        {
            User = user;
        }
    }

    public interface IGetUserDetailsPresenterCallback : ICallback<GetUserDetailsResponse> { }

    public sealed class GetUserDetails : UseCaseBase<GetUserDetailsRequest, GetUserDetailsResponse>
    {
        private readonly IGetUserDetailsDataManager DataManager;

        public GetUserDetails(GetUserDetailsRequest request, IGetUserDetailsPresenterCallback callback) : base(request, callback)
        {
            DataManager = CommonDIServiceProvider.Instance.GetService<IGetUserDetailsDataManager>();
        }

        protected override Task Action()
        {
            return DataManager.GetUserDetailsAsync(Request, new UseCaseCallback(this));
        }

        class UseCaseCallback : CallbackBase<GetUserDetailsResponse>
        {
            private readonly GetUserDetails UseCase;

            public UseCaseCallback(GetUserDetails useCase)
            {
                UseCase = useCase;
            }

            public override void OnError(UseCaseError error)
            {
                UseCase.PresenterCallback?.OnError(error);
            }

            public override void OnFailed(IUseCaseResponse<GetUserDetailsResponse> response)
            {
                UseCase.PresenterCallback?.OnFailed(response);
            }

            public override void OnSuccess(IUseCaseResponse<GetUserDetailsResponse> response)
            {
                UseCase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
}
