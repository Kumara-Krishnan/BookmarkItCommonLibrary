using BookmarkItCommonLibrary.Model.Contract;
using BookmarkItCommonLibrary.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model
{
    public class VideoBObj : Video, IAttachment1
    {
        public string LocalFileName { get; set; }

        public VideoBObj() : base() { }

        public VideoBObj(string bookmarkId, string entityId) : base(bookmarkId, entityId) { }
    }
}
