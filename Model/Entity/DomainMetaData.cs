using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model.Entity
{
    public class DomainMetaData
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public string GreyscaleLogo { get; set; }

        public DomainMetaData() { }
    }
}
