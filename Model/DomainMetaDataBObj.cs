using BookmarkItCommonLibrary.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model
{
    public class DomainMetaDataBObj : DomainMetaData
    {
        public string LogoFileName { get; set; }

        public string GreyscaleLogoFileName { get; set; }

        public DomainMetaDataBObj() : base() { }
    }
}
