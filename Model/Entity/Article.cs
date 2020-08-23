using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author kumar-4031
*
* @date - 3/1/2020 10:45:29 PM 
*/
namespace BookmarkItCommonLibrary.Model.Entity
{
    public sealed class Article
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string BookmarkId { get; set; }

        public int PageNumber { get; set; }

        public string Content { get; set; }

        public string Encoding { get; set; }

        public Article() { }

        public Article(string bookmarkId, int pageNumber)
        {
            BookmarkId = bookmarkId;
            PageNumber = pageNumber;
            Id = $"{BookmarkId}_{PageNumber}";
        }
    }
}