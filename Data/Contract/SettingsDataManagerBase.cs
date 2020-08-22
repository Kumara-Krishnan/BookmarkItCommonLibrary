using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Model;

namespace BookmarkItCommonLibrary.Data.Contract
{
    public interface ISettingsDataManager<T> where T : SettingsBase, new()
    {
        T GetSetting(string userId, string key);

        void SetSetting(T setting);
    }

    public abstract class SettingsDataManagerBase<T> : DataManagerBase, ISettingsDataManager<T> where T : SettingsBase, new()
    {
        public T GetSetting(string userId, string key)
        {
            return DBHandler.GetSetting<T>(userId, key);
        }

        public void SetSetting(T setting)
        {
            DBHandler.SetSetting(setting);
        }
    }
}
