using BookmarkItCommonLibrary.Data.Contract;
using BookmarkItCommonLibrary.Data.Parser;
using BookmarkItCommonLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.UseCase;

namespace BookmarkItCommonLibrary.Data
{
    internal sealed class GetRequestTokenDataManager : DataManagerBase, IGetRequestTokenDataManager
    {
        public async Task GetRequestTokenAsync(GetRequestTokenRequest request, ICallback<GetRequestTokenResponse> callback)
        {
            var requestTokenResponse = await NetHandler.GetRequestTokenAsync(request.CallbackUri).ConfigureAwait(false);
            var requestToken = ResponseDataParser.ParseRequestToken(requestTokenResponse);
            callback.OnSuccess(ResponseType.Network, ResponseStatus.Success, new GetRequestTokenResponse(requestToken));
        }
    }
}
