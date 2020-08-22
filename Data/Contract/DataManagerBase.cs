using BookmarkItCommonLibrary.Data.Handler.Contract;
using BookmarkItCommonLibrary.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Data.Contract
{
    public abstract class DataManagerBase
    {
        protected readonly IDBHandler DBHandler;
        protected readonly INetHandler NetHandler;

        protected DataManagerBase()
        {
            DBHandler = CommonDIServiceProvider.Instance.GetService<IDBHandler>();
            NetHandler = CommonDIServiceProvider.Instance.GetService<INetHandler>();
        }
    }
}
