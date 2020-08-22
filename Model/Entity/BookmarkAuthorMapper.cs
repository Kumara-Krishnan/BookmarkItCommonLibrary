using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model.Entity
{
    public class BookmarkAuthorMapper
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string BookmarkId { get; set; }

        public string AuthorId { get; set; }

        public BookmarkAuthorMapper() { }

        public BookmarkAuthorMapper(string bookmarkId, string authorId)
        {
            Id = $"{bookmarkId}_{authorId}";
            BookmarkId = bookmarkId;
            AuthorId = authorId;
        }
    }
}
