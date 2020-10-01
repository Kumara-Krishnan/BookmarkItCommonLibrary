using BookmarkItCommonLibrary.Data.Contract;
using BookmarkItCommonLibrary.Data.Parser;
using BookmarkItCommonLibrary.Domain;
using BookmarkItCommonLibrary.Model;
using BookmarkItCommonLibrary.Model.Entity;
using BookmarkItCommonLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.UseCase;

namespace BookmarkItCommonLibrary.Data
{
    internal class GetUserDetailsDataManager : DataManagerBase, IGetUserDetailsDataManager
    {
        public async Task GetUserDetailsAsync(GetUserDetailsRequest request, ICallback<GetUserDetailsResponse> callback)
        {
            UserDetails userDetailsFromDB = default;
            if (request.HasLocalStorage())
            {
                userDetailsFromDB = GetUserDetailsFromDB(request);
                callback.OnSuccessOrFailed(ResponseType.LocalStorage, new GetUserDetailsResponse(userDetailsFromDB), IsValidUserDetailsResponse);
            }

            if (request.HasNetwork())
            {
                var parsedUserDetails = await FetchUserDetailsFromServerAsync(request, userDetailsFromDB).ConfigureAwait(false);
                callback.OnSuccessOrFailed(ResponseType.Network, new GetUserDetailsResponse(parsedUserDetails?.User), IsValidUserDetailsResponse);
            }
        }

        public UserDetails GetUserDetailsFromDB(GetUserDetailsRequest request)
        {
            var user = DBHandler.GetUserDetails(request.UserId);
            if (request.SetAsCurrentUser) { SetAsCurrentUser(user); }
            return user;
        }

        public async Task<ParsedUserDetails> FetchUserDetailsFromServerAsync(GetUserDetailsRequest request, UserDetails userDetailsFromDB = default)
        {
            var userResponse = await NetHandler.GetAccessTokenAsync(request.RequestToken).ConfigureAwait(false);
            var userDetailsFromServer = ResponseDataParser.ParseUserDetails(userResponse);
            NetHandler.AccessToken = userDetailsFromServer.AccessToken;
            var userStat = await FetchUserStatFromServerAsync(userDetailsFromServer.User.Id).ConfigureAwait(false);
            userDetailsFromServer.User.ItemsCount = userStat.TotalItemsCount;
            userDetailsFromServer.User.UnreadItemsCount = userStat.UnreadItemsCount;
            userDetailsFromServer.User.ReadItemsCount = userStat.ReadItemsCount;

            if (userDetailsFromDB == default) { userDetailsFromDB = GetUserDetailsFromDB(request); }
            if (userDetailsFromDB != default)
            {
                userDetailsFromServer.User.IsInitialFetchComplete = userDetailsFromDB.IsInitialFetchComplete;
                userDetailsFromServer.User.IsArticleFetchComplete = userDetailsFromDB.IsArticleFetchComplete;
                userDetailsFromServer.User.InitialFetchOffset = userDetailsFromDB.InitialFetchOffset;
                userDetailsFromServer.User.LastSyncedTime = userDetailsFromDB.LastSyncedTime;
            }
            DBHandler.AddOrReplaceUserDetails(userDetailsFromServer.User);
            //CredentialLocker.StoreCredential(CredentialResourceKeys.AccessToken, userDetailsFromServer.User.Id, userDetailsFromServer.AccessToken);
            if (request.SetAsCurrentUser) { SetAsCurrentUser(userDetailsFromServer.User); }

            return userDetailsFromServer;
        }

        public async Task<UserStat> FetchUserStatFromServerAsync(string userId)
        {
            var userStatResponse = await NetHandler.GetUserStats(userId);
            var userStat = ResponseDataParser.ParseUserStat(userId, userStatResponse);
            return userStat;
        }

        private void SetAsCurrentUser(UserDetails user)
        {
            if (user == default) { return; }
            DBHandler.SetSetting(new BookmarkItSettings(CommonConstants.AppSetting, SettingKeys.CurrentUser, user.UserName));
        }

        private bool IsValidUserDetailsResponse(GetUserDetailsResponse response)
        {
            return response.User != default;
        }
    }
}
