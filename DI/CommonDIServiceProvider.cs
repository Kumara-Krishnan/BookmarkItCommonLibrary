using BookmarkItCommonLibrary.Data;
using BookmarkItCommonLibrary.Data.Contract;
using BookmarkItCommonLibrary.Data.Handler;
using BookmarkItCommonLibrary.Data.Handler.Contract;
using BookmarkItCommonLibrary.Domain;
using BookmarkItCommonLibrary.Model.Entity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Adapter.DB;
using Utilities.Adapter.DB.Contract;
using Utilities.Adapter.Net;
using Utilities.Adapter.Net.Contract;
using Utilities.Util;

namespace BookmarkItCommonLibrary.DI
{
    public sealed class CommonDIServiceProvider : DIServiceProviderBase
    {
        public static CommonDIServiceProvider Instance { get { return DIServiceProviderSingleton.Instance; } }

        private CommonDIServiceProvider() { }

        protected override void AddServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDBAdapter, SQLiteDBAdapter>();
            serviceCollection.AddSingleton<INetAdapter, NetAdapter>();

            serviceCollection.AddSingleton<IDBHandler, DBHandler>();
            serviceCollection.AddSingleton<INetHandler, NetHandler>();

            serviceCollection.AddSingleton<IGetBookmarksDataManager, GetBookmarksDataManager>();
            serviceCollection.AddSingleton<IGetCurrentUserDetailsDataManager, GetCurrentUserDetailsDataManager>();
            serviceCollection.AddSingleton<IGetRequestTokenDataManager, GetRequestTokenDataManager>();
            serviceCollection.AddSingleton<IGetUserDetailsDataManager, GetUserDetailsDataManager>();
            serviceCollection.AddSingleton<ISettingsDataManager<BookmarkItSettings>, SettingsDataManager>();
        }

        private class DIServiceProviderSingleton
        {
            internal static readonly CommonDIServiceProvider Instance = new CommonDIServiceProvider();

            static DIServiceProviderSingleton() { }
        }
    }
}
