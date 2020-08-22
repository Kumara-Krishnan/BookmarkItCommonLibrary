using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Util
{
    public enum BookmarkType
    {
        Unknown,
        Article,
        Image,
        Video
    }

    public enum VideoPresence
    {
        None,
        HasVideo,
        IsVideo
    }

    public enum ImagePresence
    {
        None,
        HasImage,
        IsImage
    }

    public enum VideoType
    {
        Unknown = 7,
        Youtube = 1,
        Vimeo = 2,
        HTML = 5,
        Flash = 6
    }

    public enum BookmarkStatus
    {
        Available,
        Archived,
        Deleted
    }

    public enum BookmarkFilter
    {
        Unread,
        Archived,
        All
    }

    public enum SortBy
    {
        Newest,
        Oldest,
        Title,
        Site
    }

    public enum BookmarkDetailDepth
    {
        Simple,
        Complete
    }
}
