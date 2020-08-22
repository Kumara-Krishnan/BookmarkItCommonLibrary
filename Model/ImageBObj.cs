using BookmarkItCommonLibrary.Model.Contract;
using BookmarkItCommonLibrary.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model
{
    public class ImageBObj : Image, IAttachment1
    {
        public string LocalFileName { get; set; }

        public ImageBObj() : base() { }

        public ImageBObj(string bookmarkId, string entityId) : base(bookmarkId, entityId) { }
    }
}
