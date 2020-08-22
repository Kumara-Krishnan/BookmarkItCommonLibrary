using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model.Entity
{
    public class Author
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public Author() { }
    }
}
