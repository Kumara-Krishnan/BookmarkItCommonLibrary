using BookmarkItCommonLibrary.Model.Contract;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model.Entity
{
    [DebuggerDisplay("Url = {Url}")]
    public class Image : IAttachment
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string BookmarkId { get; set; }

        public string EntityId { get; set; }

        public string Url { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public string Credit { get; set; }

        public string Caption { get; set; }

        public Image() { }

        public Image(string bookmarkId, string entityId)
        {
            Id = $"{bookmarkId}_{entityId}";
            BookmarkId = bookmarkId;
            EntityId = entityId;
        }
    }
}
