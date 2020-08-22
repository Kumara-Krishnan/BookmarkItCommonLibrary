using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model.Entity
{
    [DebuggerDisplay("Name = {Name}")]
    public class Tag
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string BookmarkId { get; set; }

        public string Name { get; set; }

        public Tag() { }

        public Tag(string bookmarkId, string name)
        {
            Id = $"{bookmarkId}_{name}";
            BookmarkId = bookmarkId;
            Name = name;
        }
    }
}
