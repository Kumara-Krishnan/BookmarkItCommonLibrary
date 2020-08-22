using BookmarkItCommonLibrary.Model;
using BookmarkItCommonLibrary.Model.Entity;
using BookmarkItCommonLibrary.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Utilities.Extension;

namespace BookmarkItCommonLibrary.Data.Parser
{
    public static class ResponseDataParser
    {
        public static string ParseRequestToken(string reqTokenResponse)
        {
            var jReqTokenResponse = JObject.Parse(reqTokenResponse);
            return jReqTokenResponse.GetString(JsonResponseKeys.RequestToken);
        }

        public static ParsedUserDetails ParseUserDetails(string userDetailsResponse)
        {
            var jUser = JObject.Parse(userDetailsResponse);
            var userName = jUser.GetString(JsonResponseKeys.UserName);
            var accessToken = jUser.GetString(JsonResponseKeys.AccessToken);
            var user = new UserDetails(userName);
            if (jUser.ContainsKeyAndValue(JsonResponseKeys.Account))
            {
                var jAccount = jUser.GetJObject(JsonResponseKeys.Account);
                user.PocketUserId = jAccount.GetString(JsonResponseKeys.PocketUserId);
                user.Email = jAccount.GetString(JsonResponseKeys.Email);
                user.FirstName = jAccount.GetString(JsonResponseKeys.FirstName);
                user.LastName = jAccount.GetString(JsonResponseKeys.LastName);
                user.CreationTime = jAccount.GetString(JsonResponseKeys.CreationTime);
                user.AccessToken = accessToken;
                if (jAccount.ContainsKeyAndValue(JsonResponseKeys.Profile))
                {
                    var jProfile = jAccount[JsonResponseKeys.Profile];
                    user.DisplayName = jProfile.GetString(JsonResponseKeys.DisplayName);
                    user.Description = jProfile.GetString(JsonResponseKeys.Description);
                    user.AvatarUrl = jProfile.GetString(JsonResponseKeys.AvatarUrl);
                    user.UpdatedTime = jProfile.GetString(JsonResponseKeys.UpdatedTime);
                }
            }
            return new ParsedUserDetails(user, accessToken);
        }

        public static UserStat ParseUserStat(string userId, string userStatResponse)
        {
            var jUserStat = JObject.Parse(userStatResponse);
            var userStat = new UserStat(userId)
            {
                TotalItemsCount = jUserStat.GetInt("count_list"),
                UnreadItemsCount = jUserStat.GetInt("count_unread"),
                ReadItemsCount = jUserStat.GetInt("count_read")
            };
            return userStat;
        }

        public static ParsedBookmarksResponse ParseBookmarks(string userId, string bookmarksResponse)
        {
            var parsedBookmarksResponse = new ParsedBookmarksResponse(userId);
            var jResponse = JObject.Parse(bookmarksResponse);
            parsedBookmarksResponse.Status = jResponse.GetInt("status");
            parsedBookmarksResponse.Error = jResponse.GetString("error");
            if (jResponse.ContainsKeyAndValue("search_meta"))
            {
                parsedBookmarksResponse.SearchType = jResponse.GetJObject("search_meta").GetString("search_type");
            }
            parsedBookmarksResponse.Since = jResponse.GetLong("since");
            if (jResponse.ContainsKeyAndValue("list"))
            {
                var jBookmarks = jResponse.GetJObject("list");
                var itemIds = jBookmarks.Keys();
                if (itemIds.IsNonEmpty())
                {
                    foreach (var itemId in itemIds)
                    {
                        var jBookmark = jBookmarks.GetJObject(itemId);
                        var bookmark = ParseBookmark(userId, jBookmark);
                        parsedBookmarksResponse.Add(bookmark);
                    }
                }
            }
            return parsedBookmarksResponse;
        }

        private static BookmarkBObj ParseBookmark(string userId, JObject jBookmark)
        {
            BookmarkBObj bookmark = default;
            try
            {
                bookmark = new BookmarkBObj(userId, jBookmark.GetString("item_id"))
                {
                    ResolvedId = jBookmark.GetString("resolved_id"),
                    Url = jBookmark.GetString("given_url"),
                    ResolvedUrl = jBookmark.GetString("resolved_url"),
                    Title = jBookmark.GetString("given_title"),
                    ResolvedTitle = jBookmark.GetString("resolved_title"),
                    IsFavorite = jBookmark.OptBool("favorite"),
                    Status = jBookmark.OptEnum("status", BookmarkStatus.Available),
                    CreatedTime = jBookmark.GetLong("time_added"),
                    UpdatedTime = jBookmark.GetLong("time_updated"),
                    TimeMarkedAsRead = jBookmark.GetLong("time_read"),
                    TimeFavorited = jBookmark.GetLong("time_favorited"),
                    Summary = jBookmark.GetString("excerpt"),
                    VideoPresence = jBookmark.OptEnum("has_video", VideoPresence.None),
                    ImagePresence = jBookmark.OptEnum("has_image", ImagePresence.None),
                    CoverImageUrl = jBookmark.GetString("top_image_url"),
                    WordCount = jBookmark.GetLong("word_count"),
                    Language = jBookmark.GetString("lang"),
                    TimeToRead = jBookmark.GetLong("time_to_read"),
                    TimeToListen = jBookmark.GetLong("listen_duration_estimate"),
                };
                bookmark.Type = GetBookmarkType(jBookmark.OptBool("is_article"), bookmark.ImagePresence, bookmark.VideoPresence);
                if (jBookmark.ContainsKeyAndValue("tags"))
                {
                    var tags = ParseTags(bookmark.EntityId, jBookmark.GetJObject("tags").Keys());
                    bookmark.Tags.AddRange(tags);
                }
                if (bookmark.ImagePresence != ImagePresence.None)
                {
                    var images = ParseImages(jBookmark.GetJObject("images"));
                    bookmark.SetImages(images);
                }
                if (bookmark.VideoPresence != VideoPresence.None)
                {
                    var videos = ParseVideos(jBookmark.GetJObject("videos"));
                    bookmark.SetVideos(videos);
                }
                if (jBookmark.ContainsKeyAndValue("authors"))
                {
                    var authors = ParseAuthors(jBookmark.GetJObject("authors"));
                    bookmark.SetAuthors(authors);
                }
                if (jBookmark.ContainsKeyAndValue("domain_metadata"))
                {
                    bookmark.Domain = ParseDomainMetaData(jBookmark.GetJObject("domain_metadata"));
                }
            }
            catch { }
            return bookmark;
        }

        private static BookmarkType GetBookmarkType(bool isArticle, ImagePresence imagePresence, VideoPresence videoPresence)
        {
            if (isArticle)
            {
                return BookmarkType.Article;
            }
            if (imagePresence == ImagePresence.IsImage)
            {
                return BookmarkType.Image;
            }
            if (videoPresence == VideoPresence.IsVideo)
            {
                return BookmarkType.Video;
            }
            return BookmarkType.Unknown;
        }

        private static List<Tag> ParseTags(string bookmarkId, IEnumerable<string> tagNames)
        {
            List<Tag> tags = new List<Tag>();
            foreach (var tagName in tagNames)
            {
                var tag = new Tag(bookmarkId, tagName);
                tags.Add(tag);
            }
            return tags;
        }

        private static IEnumerable<ImageBObj> ParseImages(JObject jImages)
        {
            var imageIds = jImages.Keys();
            foreach (var imageId in imageIds)
            {
                var jImage = jImages.GetJObject(imageId);
                yield return ParseImage(jImage);
            }
        }

        private static ImageBObj ParseImage(JObject jImage)
        {
            return new ImageBObj(jImage.GetString("item_id"), jImage.GetString("image_id"))
            {
                Url = jImage.GetString("src"),
                Width = jImage.GetDouble("width"),
                Height = jImage.GetDouble("height"),
                Credit = jImage.GetString("credit"),
                Caption = jImage.GetString("caption")
            };
        }

        private static IEnumerable<VideoBObj> ParseVideos(JObject jVideos)
        {
            var videoIds = jVideos.Keys();
            foreach (var videoId in videoIds)
            {
                var jVideo = jVideos.GetJObject(videoId);
                yield return ParseVideo(jVideo);
            }
        }

        private static VideoBObj ParseVideo(JObject jVideo)
        {
            return new VideoBObj(jVideo.GetString("item_id"), jVideo.GetString("video_id"))
            {
                Url = jVideo.GetString("src"),
                Width = jVideo.GetDouble("video"),
                Height = jVideo.GetDouble("height"),
                ExternalId = jVideo.GetString("vid"),
                Type = jVideo.OptEnum("type", VideoType.Unknown),
                Length = jVideo.GetLong("length")
            };
        }

        private static IEnumerable<Author> ParseAuthors(JObject jAuthors)
        {
            var authorIds = jAuthors.Keys();
            foreach (var authorId in authorIds)
            {
                var jAuthor = jAuthors.GetJObject(authorId);
                yield return ParseAuthor(jAuthor);
            }
        }

        private static Author ParseAuthor(JObject jAuthor)
        {
            return new Author()
            {
                Id = jAuthor.GetString("author_id"),
                Name = jAuthor.GetString("name"),
                Url = jAuthor.GetString("url")
            };
        }

        private static DomainMetaDataBObj ParseDomainMetaData(JObject jDomainInfo)
        {
            if (string.IsNullOrEmpty(jDomainInfo.GetString("name"))) { return default; }
            return new DomainMetaDataBObj()
            {
                Id = jDomainInfo.GetString("name"),
                Name = jDomainInfo.GetString("name"),
                Logo = jDomainInfo.GetString("logo"),
                GreyscaleLogo = jDomainInfo.GetString("greyscale_logo"),
            };
        }
    }
}
