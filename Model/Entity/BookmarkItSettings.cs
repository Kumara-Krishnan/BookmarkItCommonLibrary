using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Model;

namespace BookmarkItCommonLibrary.Model.Entity
{
    public sealed class BookmarkItSettings : SettingsBase
    {
        public BookmarkItSettings() { }

        public BookmarkItSettings(string userId, string key, string value) : base(userId, key, value)
        {

        }

        public BookmarkItSettings(string userId, string key, string value, string groupId) : base(userId, key, value, groupId)
        {

        }
    }
}
