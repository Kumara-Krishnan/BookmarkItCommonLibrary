using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model.Entity
{
    public class BookmarkDomainMapper
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string BookmarkId { get; set; }

        public string DomainId { get; set; }

        public BookmarkDomainMapper() { }

        public BookmarkDomainMapper(string bookmarkId, string domainId)
        {
            Id = $"{bookmarkId}_{domainId}";
            BookmarkId = bookmarkId;
            DomainId = domainId;
        }
    }
}
