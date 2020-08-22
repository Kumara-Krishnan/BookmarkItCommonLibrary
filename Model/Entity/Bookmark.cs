using BookmarkItCommonLibrary.Model.Contract;
using BookmarkItCommonLibrary.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model.Entity
{
    public class Bookmark : IBookmark
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string EntityId { get; set; }

        public string UserId { get; set; }

        public string ResolvedId { get; set; }

        public string Url { get; set; }

        public string ResolvedUrl { get; set; }

        public string Title { get; set; }

        public string ResolvedTitle { get; set; }

        public bool IsFavorite { get; set; }

        public string Summary { get; set; }

        public BookmarkType Type { get; set; }

        public BookmarkStatus Status { get; set; }

        public long CreatedTime { get; set; }

        public long UpdatedTime { get; set; }

        public long TimeMarkedAsRead { get; set; }

        public long TimeFavorited { get; set; }

        public ImagePresence ImagePresence { get; set; }

        public VideoPresence VideoPresence { get; set; }

        public string CoverImageUrl { get; set; }

        public long WordCount { get; set; }

        public string Language { get; set; }

        public long TimeToRead { get; set; }

        public long TimeToListen { get; set; }

        public bool IsArticleError { get; set; }

        public Bookmark() { }

        public Bookmark(string userId, string entityId)
        {
            Id = $"{userId}_{entityId}";
            UserId = userId;
            EntityId = entityId;
        }
    }
}
