using BookmarkItCommonLibrary.Model.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model
{
    [DebuggerDisplay("Url = {Url}")]
    public sealed class BookmarkBObj : Bookmark
    {
        public readonly List<Tag> Tags = new List<Tag>();

        public readonly List<ImageBObj> Images = new List<ImageBObj>();

        public readonly List<VideoBObj> Videos = new List<VideoBObj>();

        public readonly List<Author> Authors = new List<Author>();

        public DomainMetaDataBObj Domain { get; set; }

        public BookmarkBObj() : base() { }

        public BookmarkBObj(string userId, string entityId) : base(userId, entityId) { }

        public void SetTags(IEnumerable<Tag> tags)
        {
            Tags.Clear();
            if (tags == default) { return; }
            foreach (var tag in tags)
            {
                Tags.Add(tag);
            }
        }

        public void SetImages(IEnumerable<ImageBObj> images)
        {
            Images.Clear();
            if (images == default) { return; }
            foreach (var image in images)
            {
                Images.Add(image);
            }
        }

        public void SetVideos(IEnumerable<VideoBObj> videos)
        {
            Videos.Clear();
            if (videos == default) { return; }
            foreach (var video in videos)
            {
                Videos.Add(video);
            }
        }

        public void SetAuthors(IEnumerable<Author> authors)
        {
            Authors.Clear();
            if (authors == default) { return; }
            foreach (var author in authors)
            {
                Authors.Add(author);
            }
        }
    }
}
