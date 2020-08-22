using BookmarkItCommonLibrary.Data.Contract;
using BookmarkItCommonLibrary.DI;
using BookmarkItCommonLibrary.Domain;
using BookmarkItCommonLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Error;
using Utilities.Extension;
using Utilities.UseCase;

namespace BookmarkItCommonLibrary.Data
{
    internal sealed class GetCurrentUserDetailsDataManager : DataManagerBase, IGetCurrentUserDetailsDataManager
    {
        public void GetCurrentUserDetails(GetCurrentUserDetailsRequest request, ICallback<GetCurrentUserDetailsResponse> callback)
        {
            var settingsDM = CommonDIServiceProvider.Instance.GetService<ISettingsDataManager<Model.Entity.BookmarkItSettings>>();
            var currentUserId = settingsDM.GetSetting(CommonConstants.AppSetting, SettingKeys.CurrentUser).OptString();
            if (string.IsNullOrEmpty(currentUserId))
            {
                callback?.OnFailed(ResponseType.LocalStorage, ResponseStatus.Failed);
            }
            else
            {
                var currentUser = DBHandler.GetUserDetails(currentUserId);
                callback?.OnSuccessOrFailed(ResponseType.LocalStorage, new GetCurrentUserDetailsResponse(currentUser), IsValidResponse);
            }
        }

        private bool IsValidResponse(GetCurrentUserDetailsResponse response)
        {
            return response.User != default;
        }
    }
}
