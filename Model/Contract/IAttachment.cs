using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model.Contract
{
    public interface IAttachment
    {
        string Id { get; set; }

        string BookmarkId { get; set; }

        string EntityId { get; set; }

        string Url { get; set; }

        double Width { get; set; }

        double Height { get; set; }
    }

    public interface IAttachment1 : IAttachment
    {
        string LocalFileName { get; set; }
    }
}
