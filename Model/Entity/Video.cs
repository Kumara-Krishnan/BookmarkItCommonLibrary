using BookmarkItCommonLibrary.Model.Contract;
using BookmarkItCommonLibrary.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model.Entity
{
    [DebuggerDisplay("Url = {Url}, ExternalId = {ExternalId}")]
    public class Video : IAttachment
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string BookmarkId { get; set; }

        public string EntityId { get; set; }

        public string Url { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public string ExternalId { get; set; }

        public VideoType Type { get; set; }

        public long Length { get; set; }

        public Video() { }

        public Video(string bookmarkId, string entityId)
        {
            Id = $"{bookmarkId}_{entityId}";
            BookmarkId = bookmarkId;
            EntityId = entityId;
        }
    }
}
